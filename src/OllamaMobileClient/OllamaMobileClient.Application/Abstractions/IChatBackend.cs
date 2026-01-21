namespace OllamaMobileClient.Applications.Abstractions
{
    public interface IChatBackend
    {
        Task SendUserMessageAsync(string chatId, string text, CancellationToken ct);

        IAsyncEnumerable<string> StreamAssistantReplyAsync(string chatId, CancellationToken ct);
    }
}
