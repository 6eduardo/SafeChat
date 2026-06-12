namespace SafeChat.Application.DTOs.Users;

public class UserSearchResultDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PublicKey { get; set; } = string.Empty;
    public bool IsOnline { get; set; }
}
