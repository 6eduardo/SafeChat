namespace SafeChat.Mobile.Models.DTOs.Auth;

/// <summary>
/// DTO de resposta de autenticação (login e registo) devolvido pela API.
/// Contém o token JWT e dados básicos do utilizador autenticado.
/// </summary>
public class AuthResponseDto
{
    /// <summary>Token JWT para pedidos autenticados.</summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>Identificador interno do utilizador.</summary>
    public int UserId { get; set; }

    /// <summary>Nome de utilizador.</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>E-mail do utilizador.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Data/hora UTC de expiração do token.</summary>
    public DateTime ExpiresAt { get; set; }
}
