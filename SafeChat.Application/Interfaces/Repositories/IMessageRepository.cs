using SafeChat.Domain.Entities;

namespace SafeChat.Application.Interfaces.Repositories;

public interface IMessageRepository
{
    Task<Message> AddAsync(Message message, CancellationToken cancellationToken = default);
    Task UpdateConversationLastMessageAtAsync(int conversationId, DateTime sentAt, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Message> Items, int TotalCount)> GetByConversationAsync(
        int conversationId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}
