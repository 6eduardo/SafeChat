namespace SafeChat.Mobile.DTO.SignalR;

/// <summary>
/// Evento de presença (online/offline) recebido via SignalR.
/// </summary>
public class UserPresenceDto
{
    /// <summary>Identificador do utilizador.</summary>
    public int UserId { get; set; }

    /// <summary>Indica se o utilizador está online.</summary>
    public bool IsOnline { get; set; }

    /// <summary>Última vez visto (UTC), quando offline.</summary>
    public DateTime? LastSeenAt { get; set; }
}
