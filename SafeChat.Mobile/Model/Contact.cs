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

    /// <summary>Iniciais para o avatar na UI.</summary>
    public string Initials => GetInitials(Username);

    /// <summary>Texto de presença formatado para exibição na UI.</summary>
    public string PresenceText => IsOnline ? "Online" : FormatLastSeen(LastSeenAt);

    private static string GetInitials(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return "??";

        var parts = username.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length >= 2
            ? $"{parts[0][0]}{parts[^1][0]}".ToUpperInvariant()
            : username.Length >= 2
                ? username[..2].ToUpperInvariant()
                : username.ToUpperInvariant();
    }

    private static string FormatLastSeen(DateTime? lastSeenAt) =>
        lastSeenAt.HasValue ? $"Visto {lastSeenAt.Value.ToLocalTime():g}" : "Offline";
}
