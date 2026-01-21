using System.Text.Json.Serialization;

namespace OllamaMobileClient.Infrastructure.Backends.DirectOllama
{
    public sealed class OllamaChatResponseLine
    {
        [JsonPropertyName("message")]
        public OllamaMessage? Message { get; init; }

        [JsonPropertyName("done")]
        public bool Done { get; init; }
    }

}
