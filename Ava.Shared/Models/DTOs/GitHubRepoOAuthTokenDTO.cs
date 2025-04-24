namespace Ava.Shared.Models.DTOs;
public class GitHubRepoOAuthTokenDTO
{
    [Required]
    public required string Token { get; set; }
    
    [Required]
    public required string Owner { get; set; }
    
    [Required]
    public required string Repo { get; set; }
} 