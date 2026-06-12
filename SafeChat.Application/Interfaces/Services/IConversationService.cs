using SafeChat.Application.DTOs.Conversations;

namespace SafeChat.Application.Interfaces.Services;

public interface IConversationService
{
    Task<ConversationSummaryDto> CreateAsync(int currentUserId, int participantUserId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ConversationSummaryDto>> GetUserConversationsAsync(int userId, CancellationToken cancellationToken = default);
}
