namespace SafeChat.Mobile.DTO.Auth;

/// <summary>
/// Pedido de login enviado à API (<c>POST api/Auth/login</c>).
/// </summary>
public class LoginRequestDto
{
    /// <summary>E-mail ou nome de utilizador.</summary>
    public string EmailOrUsername { get; set; } = string.Empty;

    /// <summary>Palavra-passe em texto claro (enviada apenas via HTTPS).</summary>
    public string Password { get; set; } = string.Empty;
}
