using SafeChat.Mobile.Configuration;
using SafeChat.Mobile.DTO.Conversations;
using SafeChat.Mobile.Model;
using SafeChat.Mobile.Services.Auth;
using SafeChat.Mobile.Services.Interfaces;

namespace SafeChat.Mobile.Services.Api;

/// <summary>
/// Serviço REST para gestão de conversas via SafeChat.API.
/// </summary>
public class ConversationService : BaseApiService, IConversationService
{
    public ConversationService(HttpClient httpClient, ApiConfiguration configuration, TokenService tokenService)
        : base(httpClient, configuration, tokenService)
    {
    }

    public async Task<IReadOnlyList<ConversationListItem>> GetConversationsAsync(
        CancellationToken cancellationToken = default)
    {
        var summaries = await GetAsync<List<ConversationSummaryDto>>("api/conversations", cancellationToken);

        return summaries.Select(MapToListItem).ToList();
    }

    public async Task<ConversationListItem> CreateConversationAsync(
        int participantUserId,
        CancellationToken cancellationToken = default)
    {
        var payload = new CreateConversationRequestDto { ParticipantUserId = participantUserId };
        var summary = await PostAsync<ConversationSummaryDto>("api/conversations", payload, cancellationToken);
        return MapToListItem(summary);
    }

    private static ConversationListItem MapToListItem(ConversationSummaryDto dto) =>
        new()
        {
            Id = dto.Id,
            DisplayName = dto.DisplayName,
            Initials = dto.Initials,
            LastMessagePreview = dto.LastMessagePreview,
            TimestampDisplay = FormatTimestamp(dto.LastMessageAt ?? dto.CreatedAt),
            UnreadCount = 0,
            IsOnline = dto.IsOtherParticipantOnline
        };

    private static string FormatTimestamp(DateTime utc)
    {
        var local = utc.ToLocalTime();
        var today = DateTime.Today;

        if (local.Date == today)
            return local.ToString("HH:mm");

        if (local.Date == today.AddDays(-1))
            return "Ontem";

        return local.ToString("ddd");
    }
}
