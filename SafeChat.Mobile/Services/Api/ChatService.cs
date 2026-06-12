using System.Text;
using SafeChat.Mobile.Configuration;
using SafeChat.Mobile.DTO.Conversations;
using SafeChat.Mobile.DTO.Messages;
using SafeChat.Mobile.Model;
using SafeChat.Mobile.Services.Auth;
using SafeChat.Mobile.Services.Interfaces;

namespace SafeChat.Mobile.Services.Api;

/// <summary>
/// Serviço REST para histórico de mensagens e detalhes de conversas.
/// </summary>
public class ChatService : BaseApiService, IChatService
{
    public ChatService(HttpClient httpClient, ApiConfiguration configuration, TokenService tokenService)
        : base(httpClient, configuration, tokenService)
    {
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

        return page.Items
            .Select(dto => MapToChatMessage(dto, currentUserId))
            .ToList();
    }

    public ChatMessage MapToChatMessage(MessageDto dto, int currentUserId)
    {
        var isMine = dto.SenderId == currentUserId;

        return new ChatMessage
        {
            Id = dto.Id,
            ConversationId = dto.ConversationId,
            SenderId = dto.SenderId,
            SenderUsername = dto.SenderUsername,
            SenderInitials = GetInitials(dto.SenderUsername),
            Kind = isMine ? ChatMessageKind.Outgoing : ChatMessageKind.Incoming,
            Content = DecodePayload(dto.EncryptedContent),
            SentAt = dto.SentAt,
            TimestampDisplay = dto.SentAt.ToLocalTime().ToString("HH:mm")
        };
    }

    public static (string EncryptedContent, string EncryptedAesKey, string AesIv) EncodePayload(string plaintext)
    {
        var encryptedContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(plaintext));
        return (encryptedContent, "e2ee-pending", "e2ee-pending");
    }

    private async Task<ConversationSummaryDto?> FindSummaryAsync(
        int conversationId,
        CancellationToken cancellationToken)
    {
        var summaries = await GetAsync<List<ConversationSummaryDto>>(
            "api/conversations",
            cancellationToken);

        return summaries.FirstOrDefault(c => c.Id == conversationId);
    }

    private static string DecodePayload(string encryptedContent)
    {
        try
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encryptedContent));
        }
        catch (FormatException)
        {
            return "Mensagem encriptada";
        }
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
