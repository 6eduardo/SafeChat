namespace SafeChat.Mobile.Model;

/// <summary>
/// Modelo de contacto para listagens e seleção na UI.
/// </summary>
public class Contact
{
    /// <summary>Identificador do utilizador contacto.</summary>
    public int UserId { get; set; }

    /// <summary>Nome de utilizador.</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>E-mail do contacto.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Chave pública RSA-2048 em Base64.</summary>
    public string PublicKey { get; set; } = string.Empty;

    /// <summary>Indica se o contacto está online.</summary>
    public bool IsOnline { get; set; }

    /// <summary>Última vez visto (UTC).</summary>
    public DateTime? LastSeenAt { get; set; }

    /// <summary>Texto de presença formatado para exibição na UI.</summary>
    public string PresenceText => IsOnline ? "Online" : FormatLastSeen(LastSeenAt);

    private static string FormatLastSeen(DateTime? lastSeenAt) =>
        lastSeenAt.HasValue ? $"Visto {lastSeenAt.Value.ToLocalTime():g}" : "Offline";
}
