namespace SafeChat.Mobile.Models;

/// <summary>
/// Modelo local do utilizador autenticado para uso na UI e estado da aplicação.
/// </summary>
public class User
{
    /// <summary>Identificador do utilizador.</summary>
    public int Id { get; set; }

    /// <summary>Nome de utilizador.</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>E-mail do utilizador.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Indica se o utilizador está online.</summary>
    public bool IsOnline { get; set; }

    /// <summary>Data/hora UTC da última actividade.</summary>
    public DateTime? LastSeenAt { get; set; }
}
