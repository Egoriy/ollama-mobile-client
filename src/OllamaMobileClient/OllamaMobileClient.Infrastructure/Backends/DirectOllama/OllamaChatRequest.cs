using System.Text.Json.Serialization;

namespace OllamaMobileClient.Infrastructure.Backends.DirectOllama
{
    public sealed class OllamaChatRequest
    {
        [JsonPropertyName("model")]
        public required string Model { get; init; }

        [JsonPropertyName("messages")]
        public required List<OllamaMessage> Messages { get; init; }

        [JsonPropertyName("stream")]
        public bool Stream { get; init; } = true;
    }
}
