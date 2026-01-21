using OllamaMobileClient.Domain.Chats;
using System.ComponentModel;

namespace OllamaMobileClient.App.ViewModels
{
    /// <summary>
    /// Пока просто модель сюда положим, когда заработает - будем прикручивать mvvm
    /// </summary>
    public sealed class MessageVm : INotifyPropertyChanged
    {
        private string _text;

        public MessageVm(Role role, string text)
        {
            Role = role;
            Text = text;
        }

        public Role Role { get; private set; }

        public string Text
        {
            get => _text;
            set
            {
                if (_text == value) return;
                _text = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
