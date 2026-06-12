using CommunityToolkit.Mvvm.ComponentModel;

namespace SafeChat.Mobile.Model;

/// <summary>
/// Item de conversa para a listagem na UI.
/// </summary>
public partial class ConversationListItem : ObservableObject
{
  public int Id { get; set; }

  public string DisplayName { get; set; } = string.Empty;

  public string Initials { get; set; } = string.Empty;

  public string LastMessagePreview { get; set; } = "Mensagem encriptada";

  public string TimestampDisplay { get; set; } = string.Empty;

  public int UnreadCount { get; set; }

  public bool IsOnline { get; set; }

  [ObservableProperty]
  private bool _isSelected;

  public bool HasUnread => UnreadCount > 0;
}
