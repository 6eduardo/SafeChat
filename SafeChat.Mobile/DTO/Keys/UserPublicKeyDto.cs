namespace SafeChat.Mobile.DTO.Keys;

/// <summary>
/// Resposta de <c>GET api/users/{userId}/publickey</c>.
/// </summary>
public class UserPublicKeyDto
{
    public int UserId { get; set; }
    public string PublicKey { get; set; } = string.Empty;
}
