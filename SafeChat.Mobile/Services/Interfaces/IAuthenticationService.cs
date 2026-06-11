using SafeChat.Mobile.DTO.Auth;

namespace SafeChat.Mobile.Services.Interfaces;

public interface IAuthenticationService
{
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
}
