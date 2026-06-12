using Microsoft.EntityFrameworkCore;
using SafeChat.Application.Interfaces.Repositories;
using SafeChat.Domain.Entities;
using SafeChat.Infrastructure.Persistence;

namespace SafeChat.Infrastructure.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly SafeChatDbContext _context;

    public MessageRepository(SafeChatDbContext context)
    {
        _context = context;
    }

    public async Task<Message> AddAsync(Message message, CancellationToken cancellationToken = default)
    {
        _context.Messages.Add(message);
        await _context.SaveChangesAsync(cancellationToken);

        await _context.Entry(message)
            .Reference(m => m.Sender)
            .LoadAsync(cancellationToken);

        return message;
    }

    public async Task UpdateConversationLastMessageAtAsync(
        int conversationId,
        DateTime sentAt,
        CancellationToken cancellationToken = default)
    {
        var conversation = await _context.Conversations
            .FirstOrDefaultAsync(c => c.Id == conversationId, cancellationToken);

        if (conversation is null)
            return;

        conversation.LastMessageAt = sentAt;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<Message> Items, int TotalCount)> GetByConversationAsync(
        int conversationId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Messages
            .AsNoTracking()
            .Include(m => m.Sender)
            .Where(m => m.ConversationId == conversationId);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(m => m.SentAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
