using Microsoft.AspNetCore.SignalR.Client;
using SafeChat.Mobile.Configuration;
using SafeChat.Mobile.DTO.SignalR;
using SafeChat.Mobile.Services.Auth;

namespace SafeChat.Mobile.Services.RealTime;

/// <summary>
/// Ligação SignalR ao ChatHub para mensagens em tempo real.
/// </summary>
public class SignalRService : IAsyncDisposable
{
    private readonly ApiConfiguration _configuration;
    private readonly TokenService _tokenService;
    private HubConnection? _connection;
    private readonly SemaphoreSlim _connectionLock = new(1, 1);

    public SignalRService(ApiConfiguration configuration, TokenService tokenService)
    {
        _configuration = configuration;
        _tokenService = tokenService;
        _tokenService.SessionCleared += OnSessionCleared;
    }

    public event EventHandler<NewMessageNotificationDto>? MessageReceived;

    public bool IsConnected => _connection?.State == HubConnectionState.Connected;

    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        await _connectionLock.WaitAsync(cancellationToken);

        try
        {
            if (_connection?.State == HubConnectionState.Connected)
                return;

            var token = _tokenService.Token;
            if (string.IsNullOrEmpty(token))
                return;

            if (_connection is not null)
            {
                await _connection.DisposeAsync();
                _connection = null;
            }

            var hubUrl = $"{_configuration.SignalRHubUrl}?access_token={Uri.EscapeDataString(token)}";

            _connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .WithAutomaticReconnect()
                .Build();

            _connection.On<NewMessageNotificationDto>("ReceiveMessage", notification =>
            {
                MessageReceived?.Invoke(this, notification);
            });

            await _connection.StartAsync(cancellationToken);
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    public async Task DisconnectAsync()
    {
        await _connectionLock.WaitAsync();

        try
        {
            if (_connection is null)
                return;

            await _connection.StopAsync();
            await _connection.DisposeAsync();
            _connection = null;
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    public async Task EnsureConnectedAsync(CancellationToken cancellationToken = default)
    {
        if (IsConnected)
            return;

        await ConnectAsync(cancellationToken);
    }

    public async Task JoinConversationAsync(int conversationId, CancellationToken cancellationToken = default)
    {
        await EnsureConnectedAsync(cancellationToken);

        if (_connection is null)
            throw new InvalidOperationException("Ligação SignalR indisponível.");

        await _connection.InvokeAsync("JoinConversation", conversationId, cancellationToken);
    }

    public async Task SendMessageAsync(
        int conversationId,
        string encryptedContent,
        string encryptedAesKey,
        string aesIv,
        CancellationToken cancellationToken = default)
    {
        await EnsureConnectedAsync(cancellationToken);

        if (_connection is null)
            throw new InvalidOperationException("Ligação SignalR indisponível.");

        await _connection.InvokeAsync(
            "SendMessage",
            conversationId,
            encryptedContent,
            encryptedAesKey,
            aesIv,
            cancellationToken);
    }

    private void OnSessionCleared() => _ = DisconnectAsync();

    public async ValueTask DisposeAsync()
    {
        _tokenService.SessionCleared -= OnSessionCleared;
        await DisconnectAsync();
        _connectionLock.Dispose();
    }
}
