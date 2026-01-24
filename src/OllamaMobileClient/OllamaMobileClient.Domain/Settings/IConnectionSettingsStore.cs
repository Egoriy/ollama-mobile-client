namespace OllamaMobileClient.Domain.Settings
{
    public interface IConnectionSettingsStore
    {
        Task<ConnectionSettings> GetAsync(CancellationToken ct);

        Task SaveAsync(ConnectionSettings settings, CancellationToken ct);
    }

}
