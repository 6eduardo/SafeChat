namespace SafeChat.Mobile.Models.DTOs.Auth;

/// <summary>
/// DTO de pedido de login enviado para <c>POST api/Auth/login</c>.
/// </summary>
public class LoginRequestDto
{
    /// <summary>E-mail ou nome de utilizador.</summary>
    public string EmailOrUsername { get; set; } = string.Empty;

    /// <summary>Palavra-passe em texto claro (enviada apenas via HTTPS).</summary>
    public string Password { get; set; } = string.Empty;
}
