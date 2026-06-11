using SafeChat.Mobile.DTO.Auth;
using SafeChat.Mobile.Model;
using SafeChat.Mobile.Services.Auth;
using SafeChat.Mobile.Services.Crypto;
using SafeChat.Mobile.Services.Interfaces;

namespace SafeChat.Mobile.Services.Mock;

public class MockAuthenticationService : IAuthenticationService
{
    private readonly TokenService _tokenService;
    private readonly RsaKeyService _rsaKeyService;

    private static readonly List<MockUser> _users = [];
    private static int _nextId = 1;

    private static readonly TimeSpan SimulatedDelay = TimeSpan.FromMilliseconds(1200);

    public MockAuthenticationService(TokenService tokenService, RsaKeyService rsaKeyService)
    {
        _tokenService = tokenService;
        _rsaKeyService = rsaKeyService;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        await Task.Delay(SimulatedDelay);

        var user = _users.FirstOrDefault(u =>
            u.Email.Equals(request.EmailOrUsername, StringComparison.OrdinalIgnoreCase) ||
            u.Username.Equals(request.EmailOrUsername, StringComparison.OrdinalIgnoreCase));

        if (user is null)
            throw new UnauthorizedAccessException("Credenciais inválidas. Verifica o e-mail/nome de utilizador e a palavra-passe.");

        if (user.Password != request.Password)
            throw new UnauthorizedAccessException("Credenciais inválidas. Verifica o e-mail/nome de utilizador e a palavra-passe.");

        return CreateAuthResponse(user);
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        await Task.Delay(SimulatedDelay);

        if (_users.Any(u => u.Username.Equals(request.Username, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"O nome de utilizador '{request.Username}' já está em uso.");

        if (_users.Any(u => u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"O e-mail '{request.Email}' já está registado.");

        var rsaKeyPair = _rsaKeyService.GenerateKeyPair();

        var user = new MockUser
        {
            Id = _nextId++,
            Username = request.Username,
            Email = request.Email,
            Password = request.Password,
            PublicKey = rsaKeyPair.PublicKey
        };

        _users.Add(user);

        return CreateAuthResponse(user);
    }

    private AuthResponseDto CreateAuthResponse(MockUser user)
    {
        var expiresAt = DateTime.UtcNow.AddHours(24);

        var response = new AuthResponseDto
        {
            Token = GenerateFakeJwt(user),
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            ExpiresAt = expiresAt
        };

        var session = new UserSession
        {
            Token = response.Token,
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            ExpiresAt = expiresAt
        };

        _tokenService.SaveSession(session);

        return response;
    }

    private static string GenerateFakeJwt(MockUser user)
    {
        var header = Convert.ToBase64String("{\"alg\":\"HS256\",\"typ\":\"JWT\"}"u8);
        var payload = Convert.ToBase64String(
            System.Text.Encoding.UTF8.GetBytes(
                $"{{\"sub\":\"{user.Id}\",\"email\":\"{user.Email}\",\"unique_name\":\"{user.Username}\",\"iat\":{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}}}"));
        var signature = Convert.ToBase64String(
            System.Text.Encoding.UTF8.GetBytes("mock-signature"));

        return $"{header}.{payload}.{signature}";
    }

    private class MockUser
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? PublicKey { get; set; }
    }
}
