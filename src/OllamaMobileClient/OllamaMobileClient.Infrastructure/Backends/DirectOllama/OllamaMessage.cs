using System.Text.Json.Serialization;

namespace OllamaMobileClient.Infrastructure.Backends.DirectOllama
{
    public sealed class OllamaMessage
    {
        [JsonPropertyName("role")]
        public required string Role { get; init; } // "system" | "user" | "assistant"

        [JsonPropertyName("content")]
        public string Content { get; init; } = string.Empty;

        [JsonPropertyName("thinking")]
        public string? Thinking { get; init; }
    }
}
