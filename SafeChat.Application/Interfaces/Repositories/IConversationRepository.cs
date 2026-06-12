using SafeChat.Domain.Entities;

namespace SafeChat.Application.Interfaces.Repositories;

public interface IConversationRepository
{
    Task<bool> IsParticipantAsync(int conversationId, int userId, CancellationToken cancellationToken = default);
    Task<Conversation?> FindDirectConversationAsync(int userId1, int userId2, CancellationToken cancellationToken = default);
    Task<Conversation> CreateDirectConversationAsync(int userId1, int userId2, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Conversation>> GetUserConversationsAsync(int userId, CancellationToken cancellationToken = default);
    Task<Conversation?> GetByIdForUserAsync(int conversationId, int userId, CancellationToken cancellationToken = default);
}
