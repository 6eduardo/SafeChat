using SafeChat.Domain.Entities;

namespace SafeChat.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailOrUsernameAsync(string emailOrUsername, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<User> AddAsync(User user, string? publicKeyValue = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<User>> SearchAsync(
        string query,
        int excludeUserId,
        int limit,
        CancellationToken cancellationToken = default);
    Task<string?> GetPublicKeyValueByUserIdAsync(int userId, CancellationToken cancellationToken = default);
}
