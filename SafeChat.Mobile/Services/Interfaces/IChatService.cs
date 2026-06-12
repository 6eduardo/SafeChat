using SafeChat.Mobile.Model;

namespace SafeChat.Mobile.Services.Interfaces;

/// <summary>
/// Contrato para obtenção de mensagens e detalhes de uma conversa (API real ou mock).
/// </summary>
public interface IChatService
{
  Task<ConversationListItem?> GetConversationAsync(int conversationId, CancellationToken cancellationToken = default);

  Task<IReadOnlyList<ChatMessage>> GetMessagesAsync(int conversationId, CancellationToken cancellationToken = default);
}
