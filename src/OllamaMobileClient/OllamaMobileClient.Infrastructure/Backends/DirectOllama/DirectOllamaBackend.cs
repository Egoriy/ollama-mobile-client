using OllamaMobileClient.Applications.Abstractions;
using OllamaMobileClient.Domain.Chats;
using OllamaMobileClient.Domain.Settings;
using OllamaMobileClient.Infrastructure.Settings;
using System.Text;
using System.Text.Json;

namespace OllamaMobileClient.Infrastructure.Backends.DirectOllama
{
    public sealed class DirectOllamaBackend : IChatBackend
    {
        private readonly HttpClient _http;
        private readonly IConnectionSettingsStore _settingsStore;

        // пока держим историю только на время чата в памяти:
        private readonly Dictionary<string, List<OllamaMessage>> _chatHistory = new();

        private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

        public DirectOllamaBackend(HttpClient http, IConnectionSettingsStore connectionSettingsStore)
        {
            _http = http;
            _settingsStore = connectionSettingsStore;
        }

        public Task SendUserMessageAsync(string chatId, string text, CancellationToken ct)
        {
            if (!_chatHistory.TryGetValue(chatId, out var list))
            {
                list = new List<OllamaMessage>();
                _chatHistory[chatId] = list;
            }

            list.Add(new OllamaMessage { Role = "user", Content = text, Thinking = string.Empty });
            return Task.CompletedTask;
        }

        public async IAsyncEnumerable<AssistantChunk> StreamAssistantReplyAsync(string chatId, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct)
        {
            var settings = await _settingsStore.GetAsync(ct);
            if (!_chatHistory.TryGetValue(chatId, out var list))
            {
                list = new List<OllamaMessage>();
                _chatHistory[chatId] = list;
            }

            var req = new OllamaChatRequest
            {
                Model = settings.Model,
                Messages = list,
                Stream = true
            };

            using var httpReq = new HttpRequestMessage(HttpMethod.Post, $"{settings.BaseUrl.TrimEnd('/')}/api/chat")
            {
                Content = new StringContent(JsonSerializer.Serialize(req, JsonOpts), Encoding.UTF8, "application/json")
            };

            using var resp = await _http.SendAsync(httpReq, HttpCompletionOption.ResponseHeadersRead, ct);
            resp.EnsureSuccessStatusCode();

            await using var stream = await resp.Content.ReadAsStreamAsync(ct);
            using var reader = new StreamReader(stream);

            var sb = new StringBuilder();
            var sbThinking = new StringBuilder();

            string? line = null;
            while ((line = await reader.ReadLineAsync()) is not null && !ct.IsCancellationRequested)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                OllamaChatResponseLine? msg;
                try
                {
                    msg = JsonSerializer.Deserialize<OllamaChatResponseLine>(line, JsonOpts);
                }
                catch
                {
                    continue; // пропускаем мусор/неполные строки
                }

                if (msg?.Message?.Content is { Length: > 0 } chunk)
                {
                    sb.Append(chunk);
                    yield return new AssistantChunk(AssistantChunkKind.Content, chunk);
                }

                if (msg?.Message?.Thinking is { Length: > 0 } chunkThinking)
                {
                    sbThinking.Append(chunkThinking);
                    yield return new AssistantChunk(AssistantChunkKind.Thinking, chunkThinking);
                }

                if (msg?.Done == true)
                    break;
            }

            // сохраняем финальный ответ в историю
            var final = sb.ToString();
            if (!string.IsNullOrEmpty(final))
            {
                list.Add(new OllamaMessage { Role = "assistant", Content = final, Thinking = string.Empty });
            }
        }
    }
}
