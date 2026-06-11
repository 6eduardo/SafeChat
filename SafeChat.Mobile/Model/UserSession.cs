namespace SafeChat.Mobile.Model;

/// <summary>
/// Sessão do utilizador autenticado, com token JWT e dados de perfil.
/// Persistida localmente pelo <see cref="Services.Auth.TokenService"/>.
/// </summary>
public class UserSession
{
    /// <summary>Token JWT ativo.</summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>Identificador do utilizador autenticado.</summary>
    public int UserId { get; set; }

    /// <summary>Nome de utilizador.</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>E-mail do utilizador.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Data/hora UTC de expiração do token.</summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>Indica se a sessão ainda é válida.</summary>
    public bool IsValid => !string.IsNullOrEmpty(Token) && ExpiresAt > DateTime.UtcNow;
}
