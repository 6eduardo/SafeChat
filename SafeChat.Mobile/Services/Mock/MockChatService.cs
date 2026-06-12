using SafeChat.Mobile.Model;
using SafeChat.Mobile.Services.Interfaces;

namespace SafeChat.Mobile.Services.Mock;

/// <summary>
/// Fornece mensagens fictícias para desenvolvimento da UI de chat.
/// </summary>
public class MockChatService : IChatService
{
  private static readonly Dictionary<int, ConversationListItem> Conversations = new()
  {
    [1] = new ConversationListItem
    {
      Id = 1, DisplayName = "Ana Martins", Initials = "AM", IsOnline = true
    },
    [2] = new ConversationListItem
    {
      Id = 2, DisplayName = "Ricardo Costa", Initials = "RC", IsOnline = false
    },
    [3] = new ConversationListItem
    {
      Id = 3, DisplayName = "Filipa Santos", Initials = "FS", IsOnline = true
    },
    [4] = new ConversationListItem
    {
      Id = 4, DisplayName = "Miguel Pereira", Initials = "MP", IsOnline = false
    },
    [5] = new ConversationListItem
    {
      Id = 5, DisplayName = "Beatriz Lopes", Initials = "BL", IsOnline = true
    }
  };

  public async Task<ConversationListItem?> GetConversationAsync(int conversationId, CancellationToken cancellationToken = default)
  {
    await Task.Delay(200, cancellationToken);
    return Conversations.TryGetValue(conversationId, out var conversation)
      ? CloneConversation(conversation)
      : null;
  }

  public async Task<IReadOnlyList<ChatMessage>> GetMessagesAsync(int conversationId, CancellationToken cancellationToken = default)
  {
    await Task.Delay(300, cancellationToken);
    return conversationId == 1 ? GetAnaMartinsMessages() : GetGenericMessages(conversationId);
  }

  private static List<ChatMessage> GetAnaMartinsMessages() =>
  [
    new ChatMessage
    {
      Id = 1,
      ConversationId = 1,
      Kind = ChatMessageKind.System,
      Content = "Chave RSA-2048 trocada com sucesso"
    },
    new ChatMessage
    {
      Id = 2,
      ConversationId = 1,
      SenderId = 101,
      SenderInitials = "AM",
      Kind = ChatMessageKind.Incoming,
      Content = "Olá! Já testaste a nova versão da app?",
      TimestampDisplay = "14:18"
    },
    new ChatMessage
    {
      Id = 3,
      ConversationId = 1,
      SenderId = 0,
      Kind = ChatMessageKind.Outgoing,
      Content = "Sim! A encriptação está a funcionar perfeitamente 🔒",
      TimestampDisplay = "14:19"
    },
    new ChatMessage
    {
      Id = 4,
      ConversationId = 1,
      SenderId = 101,
      SenderInitials = "AM",
      Kind = ChatMessageKind.Incoming,
      Content = "Ótimo! O servidor não consegue ler nada?",
      TimestampDisplay = "14:21"
    },
    new ChatMessage
    {
      Id = 5,
      ConversationId = 1,
      SenderId = 0,
      Kind = ChatMessageKind.Outgoing,
      Content = "Exactamente. Cada msg usa AES-256 com chave única 👍",
      TimestampDisplay = "14:22"
    },
    new ChatMessage
    {
      Id = 6,
      ConversationId = 1,
      SenderId = 101,
      SenderInitials = "AM",
      Kind = ChatMessageKind.Incoming,
      Content = "Incrível! Total privacidade garantida 🛡️",
      TimestampDisplay = "14:23"
    }
  ];

  private static List<ChatMessage> GetGenericMessages(int conversationId)
  {
    if (!Conversations.TryGetValue(conversationId, out var contact))
      return [];

    return
    [
      new ChatMessage
      {
        Id = 1,
        ConversationId = conversationId,
        Kind = ChatMessageKind.System,
        Content = "Chave RSA-2048 trocada com sucesso"
      },
      new ChatMessage
      {
        Id = 2,
        ConversationId = conversationId,
        SenderId = conversationId * 100,
        SenderInitials = contact.Initials,
        Kind = ChatMessageKind.Incoming,
        Content = "Olá! Como estás?",
        TimestampDisplay = "10:15"
      },
      new ChatMessage
      {
        Id = 3,
        ConversationId = conversationId,
        SenderId = 0,
        Kind = ChatMessageKind.Outgoing,
        Content = "Tudo bem! Mensagem encriptada com sucesso 🔒",
        TimestampDisplay = "10:16"
      }
    ];
  }

  private static ConversationListItem CloneConversation(ConversationListItem source) =>
    new()
    {
      Id = source.Id,
      DisplayName = source.DisplayName,
      Initials = source.Initials,
      IsOnline = source.IsOnline,
      LastMessagePreview = source.LastMessagePreview,
      TimestampDisplay = source.TimestampDisplay,
      UnreadCount = source.UnreadCount
    };
}
