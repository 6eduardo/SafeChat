using SafeChat.Mobile.Model;
using SafeChat.Mobile.Services.Interfaces;

namespace SafeChat.Mobile.Services.Mock;

/// <summary>
/// Fornece conversas fictícias para desenvolvimento da UI.
/// </summary>
public class MockConversationService : IConversationService
{
  private static readonly IReadOnlyList<ConversationListItem> Conversations =
  [
    new ConversationListItem
    {
      Id = 1,
      DisplayName = "Ana Martins",
      Initials = "AM",
      LastMessagePreview = "Mensagem encriptada",
      TimestampDisplay = "14:23",
      UnreadCount = 3,
      IsOnline = true,
      IsSelected = true
    },
    new ConversationListItem
    {
      Id = 2,
      DisplayName = "Ricardo Costa",
      Initials = "RC",
      LastMessagePreview = "Mensagem encriptada",
      TimestampDisplay = "Seg",
      UnreadCount = 0,
      IsOnline = false
    },
    new ConversationListItem
    {
      Id = 3,
      DisplayName = "Filipa Santos",
      Initials = "FS",
      LastMessagePreview = "Mensagem encriptada",
      TimestampDisplay = "Seg",
      UnreadCount = 1,
      IsOnline = true
    },
    new ConversationListItem
    {
      Id = 4,
      DisplayName = "Miguel Pereira",
      Initials = "MP",
      LastMessagePreview = "Mensagem encriptada",
      TimestampDisplay = "Dom",
      UnreadCount = 0,
      IsOnline = false
    },
    new ConversationListItem
    {
      Id = 5,
      DisplayName = "Beatriz Lopes",
      Initials = "BL",
      LastMessagePreview = "Mensagem encriptada",
      TimestampDisplay = "Sáb",
      UnreadCount = 0,
      IsOnline = true
    }
  ];

  public async Task<IReadOnlyList<ConversationListItem>> GetConversationsAsync(CancellationToken cancellationToken = default)
  {
    await Task.Delay(300, cancellationToken);

    return Conversations.Select(c => new ConversationListItem
    {
      Id = c.Id,
      DisplayName = c.DisplayName,
      Initials = c.Initials,
      LastMessagePreview = c.LastMessagePreview,
      TimestampDisplay = c.TimestampDisplay,
      UnreadCount = c.UnreadCount,
      IsOnline = c.IsOnline,
      IsSelected = c.IsSelected
    }).ToList();
  }

  public async Task<ConversationListItem> CreateConversationAsync(
    int participantUserId,
    CancellationToken cancellationToken = default)
  {
    await Task.Delay(200, cancellationToken);
    return Conversations.First();
  }
}
