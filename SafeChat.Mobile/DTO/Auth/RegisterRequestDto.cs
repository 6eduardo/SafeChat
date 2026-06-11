namespace SafeChat.Mobile.DTO.Auth;

/// <summary>
/// Pedido de registo enviado à API (<c>POST api/Auth/register</c>).
/// </summary>
public class RegisterRequestDto
{
    /// <summary>Nome de utilizador único.</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>E-mail do utilizador.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Palavra-passe em texto claro (enviada apenas via HTTPS).</summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>Chave pública RSA-2048 em Base64, gerada no dispositivo.</summary>
    public string? PublicKey { get; set; }
}
