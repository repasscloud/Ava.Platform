namespace AvaAITerminal.Interfaces;

public interface IApiService
{
    Task<string> GetStatusAsync();
    Task<string> GetVersionAsync();
    Task<AvaClient?> GetClientAsync(int clientId);
    Task SaveClientAsync(AvaClient client);
}
