using OllamaMobileClient.Domain.Chats;

namespace OllamaMobileClient.Applications.Abstractions
{
    public interface IChatBackend
    {
        Task SendUserMessageAsync(string chatId, string text, CancellationToken ct);

        IAsyncEnumerable<AssistantChunk> StreamAssistantReplyAsync(string chatId, CancellationToken ct);
    }
}
