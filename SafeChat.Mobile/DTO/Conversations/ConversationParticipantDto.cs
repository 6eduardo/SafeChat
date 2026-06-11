namespace SafeChat.Mobile.DTO.Conversations;

/// <summary>
/// Participante de uma conversa.
/// </summary>
public class ConversationParticipantDto
{
    /// <summary>Identificador da conversa.</summary>
    public int ConversationId { get; set; }

    /// <summary>Identificador do utilizador participante.</summary>
    public int UserId { get; set; }

    /// <summary>Nome de utilizador do participante.</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>Data/hora UTC em que entrou na conversa.</summary>
    public DateTime JoinedAt { get; set; }
}
