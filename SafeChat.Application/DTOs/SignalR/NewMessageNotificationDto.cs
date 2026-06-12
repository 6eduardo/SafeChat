using SafeChat.Application.DTOs.Messages;

namespace SafeChat.Application.DTOs.SignalR;

public class NewMessageNotificationDto
{
    public int ConversationId { get; set; }
    public MessageDto Message { get; set; } = new();
}
