using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using SafeChat.Mobile.Configuration;
using SafeChat.Mobile.DTO.Common;
using SafeChat.Mobile.Services.Auth;

namespace SafeChat.Mobile.Services.Api;

/// <summary>
/// Classe base para serviços REST da SafeChat.API.
/// Centraliza HttpClient, injeção de JWT e helpers GET/POST.
/// </summary>
public abstract class BaseApiService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    protected HttpClient HttpClient { get; }
    protected ApiConfiguration Configuration { get; }
    protected TokenService TokenService { get; }

    protected BaseApiService(
        HttpClient httpClient,
        ApiConfiguration configuration,
        TokenService tokenService)
    {
        HttpClient = httpClient;
        Configuration = configuration;
        TokenService = tokenService;
    }

    protected async Task<TResponse> GetAsync<TResponse>(
        string relativePath,
        CancellationToken cancellationToken = default)
    {
        using var request = CreateRequest(HttpMethod.Get, relativePath);
        using var response = await SendAsync(request, cancellationToken);
        return await ReadSuccessResponseAsync<TResponse>(response, cancellationToken);
    }

    protected async Task<TResponse> PostAsync<TResponse>(
        string relativePath,
        object body,
        CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(body, JsonOptions);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        using var request = CreateRequest(HttpMethod.Post, relativePath, content);
        using var response = await SendAsync(request, cancellationToken);
        return await ReadSuccessResponseAsync<TResponse>(response, cancellationToken);
    }

    protected HttpRequestMessage CreateRequest(
        HttpMethod method,
        string relativePath,
        HttpContent? content = null)
    {
        var request = new HttpRequestMessage(method, relativePath);
        if (content is not null)
            request.Content = content;

        var token = TokenService.Token;
        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return request;
    }

    private async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            return await HttpClient.SendAsync(request, cancellationToken);
        }
        catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            throw new HttpRequestException("O pedido expirou. Tenta novamente.");
        }
    }

    private static async Task<TResponse> ReadSuccessResponseAsync<TResponse>(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions, cancellationToken);
            if (result is null)
                throw new ApiException("Resposta inválida do servidor.", (int)response.StatusCode);

            return result;
        }

        var message = await ExtractErrorMessageAsync(response, cancellationToken);
        throw new ApiException(message, (int)response.StatusCode);
    }

    private static async Task<string> ExtractErrorMessageAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        try
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorDto>(JsonOptions, cancellationToken);
            if (!string.IsNullOrWhiteSpace(error?.Message))
                return error.Message;
        }
        catch (JsonException)
        {
            // Ignorar — usar mensagem genérica abaixo.
        }

        return response.StatusCode switch
        {
            System.Net.HttpStatusCode.Unauthorized =>
                "Credenciais inválidas. Verifica o e-mail/nome de utilizador e a palavra-passe.",
            System.Net.HttpStatusCode.BadRequest =>
                "Pedido inválido. Verifica os dados introduzidos.",
            System.Net.HttpStatusCode.NotFound =>
                "Recurso não encontrado.",
            _ => $"Erro do servidor ({(int)response.StatusCode}). Tenta novamente mais tarde."
        };
    }
}

/// <summary>Excepção para erros HTTP devolvidos pela API.</summary>
public sealed class ApiException : Exception
{
    public int StatusCode { get; }

    public ApiException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}
