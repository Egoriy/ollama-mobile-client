namespace OllamaMobileClient.Domain.Chats
{
    public sealed class Chat
    {
        public string Id { get; }
        public string Title { get; private set; }
        private readonly List<Message> _messages = new();
        public IReadOnlyList<Message> Messages => _messages;

        public Chat(string id, string title)
        {
            Id = id;
            Title = string.IsNullOrWhiteSpace(title) ? "New chat" : title.Trim();
        }

        public void AddMessage(Message message) => _messages.Add(message);

        public void Rename(string title)
        {
            if (!string.IsNullOrWhiteSpace(title))
                Title = title.Trim();
        }
    }
}
