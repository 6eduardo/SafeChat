namespace SafeChat.Mobile.Models.DTOs.Conversations;

/// <summary>
/// DTO de conversa devolvido pela API.
/// </summary>
public class ConversationDto
{
    /// <summary>Identificador da conversa.</summary>
    public int Id { get; set; }

    /// <summary>Data/hora UTC de criação.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Data/hora UTC da última mensagem, se existir.</summary>
    public DateTime? LastMessageAt { get; set; }

    /// <summary>Participantes da conversa.</summary>
    public List<ConversationParticipantDto> Participants { get; set; } = [];
}
