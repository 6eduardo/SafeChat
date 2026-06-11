namespace SafeChat.Mobile.DTO.Auth;

/// <summary>
/// Resposta de autenticação devolvida pela API após login ou registo.
/// </summary>
public class AuthResponseDto
{
    /// <summary>Token JWT para pedidos autenticados.</summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>Identificador do utilizador autenticado.</summary>
    public int UserId { get; set; }

    /// <summary>Nome de utilizador.</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>E-mail do utilizador.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Data/hora UTC de expiração do token.</summary>
    public DateTime ExpiresAt { get; set; }
}
