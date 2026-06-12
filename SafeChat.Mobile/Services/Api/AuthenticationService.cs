using SafeChat.Mobile.Configuration;
using SafeChat.Mobile.DTO.Auth;
using SafeChat.Mobile.Model;
using SafeChat.Mobile.Services.Auth;
using SafeChat.Mobile.Services.Crypto;
using SafeChat.Mobile.Services.Interfaces;
using SafeChat.Mobile.Services.RealTime;
using SafeChat.Mobile.Services.Storage;

namespace SafeChat.Mobile.Services.Api;

/// <summary>
/// Autenticação real via SafeChat.API (registo e login).
/// </summary>
public class AuthenticationService : BaseApiService, IAuthenticationService
{
    private readonly RsaKeyService _rsaKeyService;
    private readonly SecureKeyStorageService _secureKeyStorage;
    private readonly SignalRService _signalRService;

    public AuthenticationService(
        HttpClient httpClient,
        ApiConfiguration configuration,
        TokenService tokenService,
        RsaKeyService rsaKeyService,
        SecureKeyStorageService secureKeyStorage,
        SignalRService signalRService)
        : base(httpClient, configuration, tokenService)
    {
        _rsaKeyService = rsaKeyService;
        _secureKeyStorage = secureKeyStorage;
        _signalRService = signalRService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        var keyPair = _rsaKeyService.GenerateKeyPair();
        await _secureKeyStorage.SavePrivateKeyAsync(keyPair.PrivateKey);

        var payload = new RegisterRequestDto
        {
            Username = request.Username,
            Email = request.Email,
            Password = request.Password,
            PublicKey = keyPair.PublicKey
        };

        try
        {
            var response = await PostAsync<AuthResponseDto>("api/auth/register", payload);
            PersistSession(response);
            return response;
        }
        catch (ApiException ex)
        {
            await _secureKeyStorage.ClearPrivateKeyAsync();
            throw new InvalidOperationException(ex.Message, ex);
        }
        catch (HttpRequestException ex)
        {
            await _secureKeyStorage.ClearPrivateKeyAsync();
            throw new InvalidOperationException(
                "Não foi possível contactar o servidor. Verifica a ligação à rede e se a API está a correr.",
                ex);
        }
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        try
        {
            var response = await PostAsync<AuthResponseDto>("api/auth/login", request);
            PersistSession(response);
            return response;
        }
        catch (ApiException ex)
        {
            throw new UnauthorizedAccessException(ex.Message, ex);
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException(
                "Não foi possível contactar o servidor. Verifica a ligação à rede e se a API está a correr.",
                ex);
        }
    }

    private void PersistSession(AuthResponseDto response)
    {
        TokenService.SaveSession(new UserSession
        {
            Token = response.Token,
            UserId = response.UserId,
            Username = response.Username,
            Email = response.Email,
            ExpiresAt = response.ExpiresAt
        });

        _ = ConnectSignalRAsync();
    }

    private async Task ConnectSignalRAsync()
    {
        try
        {
            await _signalRService.ConnectAsync();
        }
        catch
        {
            // A ligação em tempo real será tentada novamente ao abrir o chat.
        }
    }
}
