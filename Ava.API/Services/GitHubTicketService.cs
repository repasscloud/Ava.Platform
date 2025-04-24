using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Ava.Shared.Models.ExternalLib.GitHub;

namespace Ava.API.Services;

public class GitHubTicketService : IGitHubTicketService
{
    private readonly HttpClient _client;
    private readonly GitHubRepoConfig _repo;

    public GitHubTicketService(IHttpClientFactory httpClientFactory, IOptions<GitHubSettings> settings)
    {
        _client = httpClientFactory.CreateClient("GitHubIssuesAPI");
        var config = settings.Value;
        _repo = config.Repos.First(r => r.Name == "AvaPlatform-Issues");
    }

    private HttpRequestMessage CreateRequest(HttpMethod method, string endpoint, string? content = null)
    {
        var request = new HttpRequestMessage(method, endpoint);
        request.Headers.Authorization = new AuthenticationHeaderValue("token", _repo.ApiKey);
        request.Headers.UserAgent.ParseAdd("AvaTicketAgent/1.0");

        if (content != null)
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

        return request;
    }

    private string RepoPath => $"/repos/{_repo.Owner}/{_repo.Repo}/issues";

    public async Task<List<GitHubTicket>> GetAllOpenTicketsAsync()
    {
        var req = CreateRequest(HttpMethod.Get, $"{RepoPath}?state=open");
        var res = await _client.SendAsync(req);
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadFromJsonAsync<List<GitHubTicket>>() ?? new();
    }

    public async Task<GitHubTicket?> GetTicketAsync(int ticketNumber)
    {
        var req = CreateRequest(HttpMethod.Get, $"{RepoPath}/{ticketNumber}");
        var res = await _client.SendAsync(req);
        return res.IsSuccessStatusCode ? await res.Content.ReadFromJsonAsync<GitHubTicket>() : null;
    }

    public async Task<List<GitHubComment>> GetCommentsAsync(int ticketNumber)
    {
        var req = CreateRequest(HttpMethod.Get, $"{RepoPath}/{ticketNumber}/comments");
        var res = await _client.SendAsync(req);
        return await res.Content.ReadFromJsonAsync<List<GitHubComment>>() ?? new();
    }

    public async Task<bool> AddCommentAsync(int ticketNumber, string comment)
    {
        var body = JsonSerializer.Serialize(new { body = comment });
        var req = CreateRequest(HttpMethod.Post, $"{RepoPath}/{ticketNumber}/comments", body);
        var res = await _client.SendAsync(req);
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> CloseTicketAsync(int ticketNumber)
    {
        var body = JsonSerializer.Serialize(new { state = "closed" });
        var req = CreateRequest(HttpMethod.Patch, $"{RepoPath}/{ticketNumber}", body);
        var res = await _client.SendAsync(req);
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> ReopenTicketAsync(int ticketNumber)
    {
        var body = JsonSerializer.Serialize(new { state = "open" });
        var req = CreateRequest(HttpMethod.Patch, $"{RepoPath}/{ticketNumber}", body);
        var res = await _client.SendAsync(req);
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> ReplaceTagAsync(int ticketNumber, string fromTag, string toTag)
    {
        var ticket = await GetTicketAsync(ticketNumber);
        if (ticket == null) return false;

        var newLabels = ticket.Labels
            .Select(l => l.Name)
            .Where(l => l != fromTag)
            .Append(toTag)
            .ToList();

        var body = JsonSerializer.Serialize(new { labels = newLabels });
        var req = CreateRequest(HttpMethod.Patch, $"{RepoPath}/{ticketNumber}", body);
        var res = await _client.SendAsync(req);
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteTicketAsync(int ticketNumber)
    {
        var ticket = await GetTicketAsync(ticketNumber);
        if (ticket == null) return false;
        await CloseTicketAsync(ticketNumber);
        var tagToRemove = ticket.Labels.FirstOrDefault(l => l.Name.StartsWith("cat-") || l.Name.StartsWith("p-"))?.Name ?? "";
        return await ReplaceTagAsync(ticketNumber, tagToRemove, "deleted");
    }

    public async Task<List<GitHubTicket>> GetTicketsByCategoryAsync(string category)
        => (await GetAllOpenTicketsAsync()).Where(t => t.Labels.Any(l => l.Name == category)).ToList();

    public async Task<List<GitHubTicket>> GetTicketsByPriorityAsync(string priority)
        => (await GetAllOpenTicketsAsync()).Where(t => t.Labels.Any(l => l.Name == priority)).ToList();

    public async Task<bool> ReassignTicketAsync(int ticketNumber)
    {
        var body = JsonSerializer.Serialize(new { assignees = new[] { "danijeljw-rpc" } });
        var req = CreateRequest(HttpMethod.Patch, $"{RepoPath}/{ticketNumber}", body);
        var res = await _client.SendAsync(req);
        return res.IsSuccessStatusCode;
    }
}
