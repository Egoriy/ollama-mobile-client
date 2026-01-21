namespace OllamaMobileClient.Infrastructure.Settings
{
    public sealed class OllamaConnection
    {
        public string BaseUrl { get; set; } = "http://192.168.1.241:11434";
        public string Model { get; set; } = "qwen3:8b";
    }

}
