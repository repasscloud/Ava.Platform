namespace AvaAITerminal.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AvaClient?> GetClientAsync(int clientId)
    {
        var endpoint = $"api/AvaClient/{clientId}";
        return await _httpClient.GetFromJsonAsync<AvaClient>(endpoint);
    }

    public Task<string> GetStatusAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetVersionAsync()
    {
        return await _httpClient.GetStringAsync("/version");
    }

    public async Task SaveClientAsync(AvaClient client)
    {
        var response = await _httpClient.PostAsJsonAsync("api/AvaClient", client);
        response.EnsureSuccessStatusCode();
    }

}
