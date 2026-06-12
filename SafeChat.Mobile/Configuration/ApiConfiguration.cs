namespace SafeChat.Mobile.Configuration;

/// <summary>
/// Configuração centralizada dos endpoints e parâmetros de comunicação com a SafeChat.API.
/// </summary>
public class ApiConfiguration
{
    /// <summary>URL base da API REST (sem barra final).</summary>
    public string BaseUrl { get; set; } = "http://192.168.0.172:5000";

    /// <summary>URL do hub SignalR (reservado para integração futura).</summary>
    public string SignalRHubUrl => $"{BaseUrl.TrimEnd('/')}/hubs/chat";

    /// <summary>Timeout máximo por pedido HTTP.</summary>
    public TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(30);
}
