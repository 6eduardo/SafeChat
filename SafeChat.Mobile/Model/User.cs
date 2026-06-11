namespace SafeChat.Mobile.Model;

/// <summary>
/// Modelo de utilizador para uso local na aplicação (UI e estado).
/// Não contém dados sensíveis do servidor (ex.: hash de password).
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

    /// <summary>Última vez visto (UTC).</summary>
    public DateTime? LastSeenAt { get; set; }
}
