namespace OllamaMobileClient.Domain.Chats
{
    public sealed record AssistantChunk(
        AssistantChunkKind Kind,
        string Text
    );
}
