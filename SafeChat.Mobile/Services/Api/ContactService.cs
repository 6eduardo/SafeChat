using SafeChat.Mobile.Configuration;
using SafeChat.Mobile.Services.Auth;

namespace SafeChat.Mobile.Services.Api;

/// <summary>
/// Serviço REST para gestão de contactos e pesquisa de utilizadores.
/// Permite adicionar contactos e obter chaves públicas RSA para troca de chaves AES.
/// </summary>
public class ContactService : BaseApiService
{
    public ContactService(HttpClient httpClient, ApiConfiguration configuration, TokenService tokenService)
        : base(httpClient, configuration, tokenService)
    {
    }
}
