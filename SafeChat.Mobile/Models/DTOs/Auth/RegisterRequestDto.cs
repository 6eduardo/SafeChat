namespace SafeChat.Mobile.Models.DTOs.Auth;

/// <summary>
/// DTO de pedido de registo enviado para <c>POST api/Auth/register</c>.
/// Inclui a chave pública RSA-2048 gerada localmente no dispositivo.
/// </summary>
public class RegisterRequestDto
{
    /// <summary>Nome de utilizador único.</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>Endereço de e-mail único.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Palavra-passe em texto claro (enviada apenas via HTTPS).</summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>Chave pública RSA-2048 em Base64, gerada no dispositivo.</summary>
    public string? PublicKey { get; set; }
}
