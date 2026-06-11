namespace SafeChat.Mobile.Models.DTOs.Conversations;

/// <summary>
/// DTO de participante numa conversa.
/// </summary>
public class ConversationParticipantDto
{
    /// <summary>Identificador da conversa.</summary>
    public int ConversationId { get; set; }

    /// <summary>Identificador do utilizador participante.</summary>
    public int UserId { get; set; }

    /// <summary>Nome de utilizador do participante (dados desnormalizados para UI).</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>Data/hora UTC em que o utilizador entrou na conversa.</summary>
    public DateTime JoinedAt { get; set; }
}
