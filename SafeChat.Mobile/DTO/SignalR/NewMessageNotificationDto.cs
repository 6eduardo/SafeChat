using SafeChat.Mobile.DTO.Messages;

namespace SafeChat.Mobile.DTO.SignalR;

/// <summary>
/// Notificação em tempo real de nova mensagem recebida via SignalR.
/// </summary>
public class NewMessageNotificationDto
{
    /// <summary>Identificador da conversa.</summary>
    public int ConversationId { get; set; }

    /// <summary>Mensagem encriptada recebida.</summary>
    public MessageDto Message { get; set; } = new();
}
