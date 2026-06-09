using SafeChat.Domain.Entities;

namespace SafeChat.Application.Interfaces.Services;

public interface IJwtTokenService
{
    (string Token, DateTime ExpiresAt) GenerateToken(User user);
}
