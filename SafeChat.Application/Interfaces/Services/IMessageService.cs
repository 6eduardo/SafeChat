using SafeChat.Application.DTOs.Messages;

namespace SafeChat.Application.Interfaces.Services;

public interface IMessageService
{
    Task<MessageDto> SendAsync(
        int conversationId,
        int senderId,
        string encryptedContent,
        string encryptedAesKey,
        string aesIv,
        CancellationToken cancellationToken = default);

    Task<PagedMessagesDto> GetConversationMessagesAsync(
        int conversationId,
        int userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}
