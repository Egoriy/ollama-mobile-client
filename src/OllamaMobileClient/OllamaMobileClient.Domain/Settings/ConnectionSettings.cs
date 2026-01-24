namespace OllamaMobileClient.Domain.Settings
{
    public sealed record ConnectionSettings(
    string Host,   // ollama.local или IP
    int Port,      // 11434
    string Model,  // qwen3:8b и т.п.
    bool ShowThinking
    )
    {
        public string BaseUrl => $"http://{Host}:{Port}";
    }

}
