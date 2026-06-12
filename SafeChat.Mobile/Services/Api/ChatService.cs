using System.Security.Cryptography;
using SafeChat.Mobile.Configuration;
using SafeChat.Mobile.DTO.Conversations;
using SafeChat.Mobile.DTO.Keys;
using SafeChat.Mobile.DTO.Messages;
using SafeChat.Mobile.Model;
using SafeChat.Mobile.Services.Auth;
using SafeChat.Mobile.Services.Crypto;
using SafeChat.Mobile.Services.Interfaces;
using SafeChat.Mobile.Services.Storage;

namespace SafeChat.Mobile.Services.Api;

/// <summary>
/// Serviço REST para histórico de mensagens com desencriptação E2EE local.
/// </summary>
public class ChatService : BaseApiService, IChatService
{
    private const string SentMessagePlaceholder = "Mensagem encriptada";
    private const string DecryptionFailedText = "Não foi possível desencriptar";

    private readonly MessageEncryptionService _messageEncryptionService;
    private readonly SecureKeyStorageService _secureKeyStorageService;

    public ChatService(
        HttpClient httpClient,
        ApiConfiguration configuration,
        TokenService tokenService,
        MessageEncryptionService messageEncryptionService,
        SecureKeyStorageService secureKeyStorageService)
        : base(httpClient, configuration, tokenService)
    {
        _messageEncryptionService = messageEncryptionService;
        _secureKeyStorageService = secureKeyStorageService;
    }

    public async Task<ConversationListItem?> GetConversationAsync(
        int conversationId,
        CancellationToken cancellationToken = default)
    {
        var summary = await FindSummaryAsync(conversationId, cancellationToken);
        if (summary is null)
            return null;

        return new ConversationListItem
        {
            Id = summary.Id,
            DisplayName = summary.DisplayName,
            Initials = summary.Initials,
            IsOnline = summary.IsOtherParticipantOnline,
            OtherParticipantUserId = summary.OtherParticipantUserId,
            LastMessagePreview = summary.LastMessagePreview,
            TimestampDisplay = summary.LastMessageAt?.ToLocalTime().ToString("HH:mm") ?? string.Empty
        };
    }

    public async Task<IReadOnlyList<ChatMessage>> GetMessagesAsync(
        int conversationId,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = TokenService.GetSession()?.UserId ?? 0;
        var page = await GetAsync<PagedMessagesDto>(
            $"api/messages/{conversationId}?page=1&pageSize=100",
            cancellationToken);

        var messages = new List<ChatMessage>();
        foreach (var dto in page.Items)
            messages.Add(await MapToChatMessageAsync(dto, currentUserId, cancellationToken));

        return messages;
    }

    public async Task<string> GetUserPublicKeyAsync(int userId, CancellationToken cancellationToken = default)
    {
        var response = await GetAsync<UserPublicKeyDto>(
            $"api/users/{userId}/publickey",
            cancellationToken);

        return response.PublicKey;
    }

    public async Task<ChatMessage> MapToChatMessageAsync(
        MessageDto dto,
        int currentUserId,
        CancellationToken cancellationToken = default)
    {
        var isMine = dto.SenderId == currentUserId;
        var content = isMine
            ? SentMessagePlaceholder
            : await DecryptMessageContentAsync(dto, cancellationToken);

        return BuildChatMessage(dto, content, isMine);
    }

    public ChatMessage BuildOutgoingMessage(string plaintext, int conversationId, int temporaryId = 0)
    {
        var session = TokenService.GetSession();
        var username = session?.Username ?? string.Empty;

        return new ChatMessage
        {
            Id = temporaryId,
            ConversationId = conversationId,
            SenderId = session?.UserId ?? 0,
            SenderUsername = username,
            SenderInitials = GetInitials(username),
            Kind = ChatMessageKind.Outgoing,
            Content = plaintext,
            SentAt = DateTime.UtcNow,
            TimestampDisplay = DateTime.Now.ToString("HH:mm")
        };
    }

    private async Task<string> DecryptMessageContentAsync(
        MessageDto dto,
        CancellationToken cancellationToken)
    {
        var privateKey = await _secureKeyStorageService.GetPrivateKeyAsync(cancellationToken);
        if (string.IsNullOrEmpty(privateKey))
            return DecryptionFailedText;

        try
        {
            return _messageEncryptionService.DecryptMessage(
                dto.EncryptedContent,
                dto.EncryptedAesKey,
                dto.AesIv,
                privateKey);
        }
        catch (CryptographicException)
        {
            return DecryptionFailedText;
        }
    }

    private static ChatMessage BuildChatMessage(MessageDto dto, string content, bool isMine) =>
        new()
        {
            Id = dto.Id,
            ConversationId = dto.ConversationId,
            SenderId = dto.SenderId,
            SenderUsername = dto.SenderUsername,
            SenderInitials = GetInitials(dto.SenderUsername),
            Kind = isMine ? ChatMessageKind.Outgoing : ChatMessageKind.Incoming,
            Content = content,
            SentAt = dto.SentAt,
            TimestampDisplay = dto.SentAt.ToLocalTime().ToString("HH:mm")
        };

    private async Task<ConversationSummaryDto?> FindSummaryAsync(
        int conversationId,
        CancellationToken cancellationToken)
    {
        var summaries = await GetAsync<List<ConversationSummaryDto>>(
            "api/conversations",
            cancellationToken);

        return summaries.FirstOrDefault(c => c.Id == conversationId);
    }

    private static string GetInitials(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return "??";

        var parts = username.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length >= 2
            ? $"{parts[0][0]}{parts[^1][0]}".ToUpperInvariant()
            : username.Length >= 2
                ? username[..2].ToUpperInvariant()
                : username.ToUpperInvariant();
    }
}
