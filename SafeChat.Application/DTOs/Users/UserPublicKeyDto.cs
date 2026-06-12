namespace SafeChat.Application.DTOs.Users;

public class UserPublicKeyDto
{
    public int UserId { get; set; }
    public string PublicKey { get; set; } = string.Empty;
}
