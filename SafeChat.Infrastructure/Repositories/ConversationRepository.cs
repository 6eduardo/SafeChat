using Microsoft.EntityFrameworkCore;
using SafeChat.Application.Interfaces.Repositories;
using SafeChat.Domain.Entities;
using SafeChat.Infrastructure.Persistence;

namespace SafeChat.Infrastructure.Repositories;

public class ConversationRepository : IConversationRepository
{
    private readonly SafeChatDbContext _context;

    public ConversationRepository(SafeChatDbContext context)
    {
        _context = context;
    }

    public Task<bool> IsParticipantAsync(int conversationId, int userId, CancellationToken cancellationToken = default)
    {
        return _context.ConversationParticipants
            .AnyAsync(cp => cp.ConversationId == conversationId && cp.UserId == userId, cancellationToken);
    }

    public Task<Conversation?> FindDirectConversationAsync(
        int userId1,
        int userId2,
        CancellationToken cancellationToken = default)
    {
        return _context.Conversations
            .Include(c => c.Participants).ThenInclude(p => p.User)
            .Include(c => c.Messages)
            .Where(c =>
                c.Participants.Count == 2 &&
                c.Participants.Any(p => p.UserId == userId1) &&
                c.Participants.Any(p => p.UserId == userId2))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Conversation> CreateDirectConversationAsync(
        int userId1,
        int userId2,
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var conversation = new Conversation
        {
            CreatedAt = now,
            Participants =
            [
                new ConversationParticipant { UserId = userId1, JoinedAt = now },
                new ConversationParticipant { UserId = userId2, JoinedAt = now }
            ]
        };

        _context.Conversations.Add(conversation);
        await _context.SaveChangesAsync(cancellationToken);

        return (await FindDirectConversationAsync(userId1, userId2, cancellationToken))!;
    }

    public async Task<IReadOnlyList<Conversation>> GetUserConversationsAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Conversations
            .AsNoTracking()
            .Include(c => c.Participants).ThenInclude(p => p.User)
            .Include(c => c.Messages)
            .Where(c => c.Participants.Any(p => p.UserId == userId))
            .OrderByDescending(c => c.LastMessageAt ?? c.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public Task<Conversation?> GetByIdForUserAsync(
        int conversationId,
        int userId,
        CancellationToken cancellationToken = default)
    {
        return _context.Conversations
            .AsNoTracking()
            .Include(c => c.Participants).ThenInclude(p => p.User)
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(
                c => c.Id == conversationId && c.Participants.Any(p => p.UserId == userId),
                cancellationToken);
    }
}
