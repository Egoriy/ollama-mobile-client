using OllamaMobileClient.App.ViewModels;
using OllamaMobileClient.Applications.Abstractions;
using OllamaMobileClient.Domain.Chats;
using System.Collections.ObjectModel;

namespace OllamaMobileClient.App
{
    public partial class MainPage : ContentPage
    {
        private readonly IChatBackend _backend;
        private readonly string _chatId = "default";

        private CancellationTokenSource? _cts;

        public ObservableCollection<MessageVm> Messages { get; } = new();

        public MainPage(IChatBackend backend)
        {
            InitializeComponent();
            _backend = backend;

            MessagesView.ItemsSource = Messages;
        }

        private async void OnSendClicked(object sender, EventArgs e)
        {
            var text = Input.Text?.Trim();
            if (string.IsNullOrEmpty(text)) return;

            Input.Text = "";

            Messages.Add(new MessageVm(Role.User, text));

            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            try
            {
                await _backend.SendUserMessageAsync(_chatId, text, _cts.Token);

                // создаём “пустое” сообщение ассистента и дописываем в него по мере стрима
                var assistant = new MessageVm(Role.Assistant, "");
                Messages.Add(assistant);

                await foreach (var chunk in _backend.StreamAssistantReplyAsync(_chatId, _cts.Token))
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        assistant.Text += chunk;
                    });

                    // лёгкий трюк, чтобы обновлялся UI:
                    var idx = Messages.IndexOf(assistant);
                    if (idx >= 0)
                    {
                        Messages[idx] = assistant;
                    }
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                Messages.Add(new MessageVm(Role.System, $"Error: {ex.Message}"));
            }
        }

        private void OnStopClicked(object sender, EventArgs e)
        {
            _cts?.Cancel();
        }
    }
}
