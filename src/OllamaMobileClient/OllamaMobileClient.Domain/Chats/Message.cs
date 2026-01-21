namespace OllamaMobileClient.Domain.Chats
{
    public sealed record Message(Role Role, string Content, DateTimeOffset CreatedAt);
}
