using SafeChat.Mobile.Model;

namespace SafeChat.Mobile.Services.Interfaces;

/// <summary>
/// Contrato para obtenção de conversas (API real ou mock).
/// </summary>
public interface IConversationService
{
  Task<IReadOnlyList<ConversationListItem>> GetConversationsAsync(CancellationToken cancellationToken = default);

  Task<ConversationListItem> CreateConversationAsync(
    int participantUserId,
    CancellationToken cancellationToken = default);
}
