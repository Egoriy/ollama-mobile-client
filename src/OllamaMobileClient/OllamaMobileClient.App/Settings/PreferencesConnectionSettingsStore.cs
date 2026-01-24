using OllamaMobileClient.Domain.Settings;

namespace OllamaMobileClient.App.Settings
{
    public sealed class PreferencesConnectionSettingsStore : IConnectionSettingsStore
    {
        private const string HostKey = "conn.host";
        private const string PortKey = "conn.port";
        private const string ModelKey = "conn.model";
        private const string ThinkingKey = "conn.showThinking";

        public Task<ConnectionSettings> GetAsync(CancellationToken ct)
        {
            // значения по умолчанию
            var host = Preferences.Default.Get(HostKey, "ollama.local");
            var port = Preferences.Default.Get(PortKey, 11434);
            var model = Preferences.Default.Get(ModelKey, "qwen3:8b");
            var showThinking = Preferences.Default.Get(ThinkingKey, false);

            var settings = new ConnectionSettings(host, port, model, showThinking);
            return Task.FromResult(settings);
        }

        public Task SaveAsync(ConnectionSettings settings, CancellationToken ct)
        {
            Preferences.Default.Set(HostKey, settings.Host);
            Preferences.Default.Set(PortKey, settings.Port);
            Preferences.Default.Set(ModelKey, settings.Model);
            Preferences.Default.Set(ThinkingKey, settings.ShowThinking);

            return Task.CompletedTask;
        }
    }
}
