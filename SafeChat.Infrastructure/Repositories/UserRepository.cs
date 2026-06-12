using Microsoft.EntityFrameworkCore;
using SafeChat.Application.Interfaces.Repositories;
using SafeChat.Domain.Entities;
using SafeChat.Infrastructure.Persistence;

namespace SafeChat.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SafeChatDbContext _context;

    public UserRepository(SafeChatDbContext context)
    {
        _context = context;
    }

    public Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant(), cancellationToken);
    }

    public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
    }

    public Task<User?> GetByEmailOrUsernameAsync(string emailOrUsername, CancellationToken cancellationToken = default)
    {
        var normalized = emailOrUsername.Trim();
        var normalizedEmail = normalized.ToLowerInvariant();

        return _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                u => u.Email == normalizedEmail || u.Username == normalized,
                cancellationToken);
    }

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return _context.Users.AnyAsync(u => u.Email == email.ToLowerInvariant(), cancellationToken);
    }

    public Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return _context.Users.AnyAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<User> AddAsync(User user, string? publicKeyValue = null, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(publicKeyValue))
        {
            user.PublicKey = new PublicKey
            {
                KeyValue = publicKeyValue.Trim(),
                CreatedAt = DateTime.UtcNow
            };
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return user;
    }

    public async Task<IReadOnlyList<User>> SearchAsync(
        string query,
        int excludeUserId,
        int limit,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return [];

        var term = query.Trim();

        return await _context.Users
            .AsNoTracking()
            .Include(u => u.PublicKey)
            .Where(u =>
                u.Id != excludeUserId &&
                (EF.Functions.Like(u.Username, $"%{term}%") ||
                 EF.Functions.Like(u.Email, $"%{term}%")))
            .OrderBy(u => u.Username)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}
