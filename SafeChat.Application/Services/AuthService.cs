using FluentValidation;
using SafeChat.Application.DTOs.Auth;
using SafeChat.Application.Exceptions;
using SafeChat.Application.Interfaces.Repositories;
using SafeChat.Application.Interfaces.Services;
using SafeChat.Domain.Entities;

namespace SafeChat.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IValidator<RegisterRequest> _registerValidator;
    private readonly IValidator<LoginRequest> _loginValidator;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService,
        IValidator<RegisterRequest> registerValidator,
        IValidator<LoginRequest> loginValidator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        await ValidateAsync(_registerValidator, request, cancellationToken);

        if (await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            throw new AuthException("Já existe uma conta com este e-mail.");

        if (await _userRepository.ExistsByUsernameAsync(request.Username, cancellationToken))
            throw new AuthException("Já existe uma conta com este nome de utilizador.");

        var user = new User
        {
            Username = request.Username.Trim(),
            Email = request.Email.Trim().ToLowerInvariant(),
            PasswordHash = _passwordHasher.Hash(request.Password),
            CreatedAt = DateTime.UtcNow
        };

        var createdUser = await _userRepository.AddAsync(user, request.PublicKey, cancellationToken);
        return BuildAuthResponse(createdUser);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        await ValidateAsync(_loginValidator, request, cancellationToken);

        var user = await _userRepository.GetByEmailOrUsernameAsync(request.EmailOrUsername.Trim(), cancellationToken);

        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new AuthException("E-mail/nome de utilizador ou palavra-passe incorrectos.");

        return BuildAuthResponse(user);
    }

    private AuthResponse BuildAuthResponse(User user)
    {
        var (token, expiresAt) = _jwtTokenService.GenerateToken(user);

        return new AuthResponse
        {
            Token = token,
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            ExpiresAt = expiresAt
        };
    }

    private static async Task ValidateAsync<T>(IValidator<T> validator, T instance, CancellationToken cancellationToken)
    {
        var result = await validator.ValidateAsync(instance, cancellationToken);
        if (!result.IsValid)
            throw new AuthException(string.Join(" ", result.Errors.Select(e => e.ErrorMessage)));
    }
}
