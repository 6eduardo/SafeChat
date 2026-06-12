using SafeChat.Application.DTOs.Users;
using SafeChat.Application.Exceptions;
using SafeChat.Application.Interfaces.Repositories;
using SafeChat.Application.Interfaces.Services;

namespace SafeChat.Application.Services;

public class UserService : IUserService
{
    private const int MaxResults = 20;

    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IReadOnlyList<UserSearchResultDto>> SearchUsersAsync(
        int currentUserId,
        string query,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return [];

        var users = await _userRepository.SearchAsync(
            query.Trim(),
            currentUserId,
            MaxResults,
            cancellationToken);

        return users.Select(u => new UserSearchResultDto
        {
            UserId = u.Id,
            Username = u.Username,
            Email = u.Email,
            PublicKey = u.PublicKey?.KeyValue ?? string.Empty,
            IsOnline = u.IsOnline
        }).ToList();
    }

    public async Task<UserPublicKeyDto> GetUserPublicKeyAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            throw new ChatException("Utilizador não encontrado.");

        var publicKey = await _userRepository.GetPublicKeyValueByUserIdAsync(userId, cancellationToken);
        if (string.IsNullOrWhiteSpace(publicKey))
            throw new ChatException("Chave pública não disponível para este utilizador.");

        return new UserPublicKeyDto
        {
            UserId = userId,
            PublicKey = publicKey
        };
    }
}
