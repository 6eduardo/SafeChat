namespace SafeChat.Mobile.DTO.Conversations;

/// <summary>
/// Resumo de conversa devolvido por <c>GET api/conversations</c>.
/// </summary>
public class ConversationSummaryDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastMessageAt { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Initials { get; set; } = string.Empty;
    public int OtherParticipantUserId { get; set; }
    public bool IsOtherParticipantOnline { get; set; }
    public string LastMessagePreview { get; set; } = "Mensagem encriptada";
}
