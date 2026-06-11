namespace SafeChat.Mobile.Services.Api;

/// <summary>
/// Serviço REST para autenticação de utilizadores (registo, login e logout).
/// Comunica com <c>api/Auth</c> e delega a persistência de tokens ao <see cref="Auth.TokenService"/>.
/// </summary>
public class AuthenticationService : BaseApiService
{
}
