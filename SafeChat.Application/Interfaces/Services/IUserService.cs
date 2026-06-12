using SafeChat.Application.DTOs.Users;

namespace SafeChat.Application.Interfaces.Services;

public interface IUserService
{
    Task<IReadOnlyList<UserSearchResultDto>> SearchUsersAsync(
        int currentUserId,
        string query,
        CancellationToken cancellationToken = default);
}
