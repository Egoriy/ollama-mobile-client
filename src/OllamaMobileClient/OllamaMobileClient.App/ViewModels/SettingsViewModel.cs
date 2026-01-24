using OllamaMobileClient.Domain.Settings;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OllamaMobileClient.App.ViewModels
{
    public sealed class SettingsViewModel : INotifyPropertyChanged
    {
        private readonly IConnectionSettingsStore _store;

        public event PropertyChangedEventHandler? PropertyChanged;

        private string _host = "ollama.local";
        public string Host { get => _host; set { _host = value; OnChanged(); } }

        private int _port = 11434;
        public int Port { get => _port; set { _port = value; OnChanged(); } }

        private string _model = "qwen3:8b";
        public string Model { get => _model; set { _model = value; OnChanged(); } }

        private bool _showThinking;
        public bool ShowThinking { get => _showThinking; set { _showThinking = value; OnChanged(); } }

        private string _status = "";
        public string Status { get => _status; private set { _status = value; OnChanged(); } }

        public SettingsViewModel(IConnectionSettingsStore store)
        {
            _store = store;
        }

        public async Task LoadAsync(CancellationToken ct)
        {
            var s = await _store.GetAsync(ct);
            Host = s.Host;
            Port = s.Port;
            Model = s.Model;
            ShowThinking = s.ShowThinking;
            Status = "";
        }

        public async Task SaveAsync(CancellationToken ct)
        {
            var host = (Host ?? "").Trim();
            if (string.IsNullOrWhiteSpace(host))
            {
                Status = "Host не может быть пустым.";
                return;
            }

            if (Port <= 0 || Port > 65535)
            {
                Status = "Port должен быть 1..65535.";
                return;
            }

            var model = (Model ?? "").Trim();
            if (string.IsNullOrWhiteSpace(model))
            {
                Status = "Model не может быть пустой.";
                return;
            }

            var s = new ConnectionSettings(host, Port, model, ShowThinking);
            await _store.SaveAsync(s, ct);
            Status = "Сохранено.";
        }

        private void OnChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
