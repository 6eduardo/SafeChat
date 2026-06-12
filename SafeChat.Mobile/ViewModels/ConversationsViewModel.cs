using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SafeChat.Mobile.Model;
using SafeChat.Mobile.Services.Auth;
using SafeChat.Mobile.Services.Interfaces;

namespace SafeChat.Mobile.ViewModels;

public partial class ConversationsViewModel : BaseViewModel
{
  private readonly IConversationService _conversationService;
  private readonly TokenService _tokenService;
  private List<ConversationListItem> _allConversations = [];

  [ObservableProperty]
  private string _searchText = string.Empty;

  [ObservableProperty]
  private string _userInitials = "JS";

  [ObservableProperty]
  private bool _isLoading;

  [ObservableProperty]
  private ConversationListItem? _selectedConversation;

  public ObservableCollection<ConversationListItem> Conversations { get; } = [];

  public ConversationsViewModel(IConversationService conversationService, TokenService tokenService)
  {
    _conversationService = conversationService;
    _tokenService = tokenService;
  }

  [RelayCommand]
  private async Task LoadConversationsAsync()
  {
    if (IsLoading)
      return;

    IsLoading = true;

    try
    {
      UserInitials = GetUserInitials();
      _allConversations = (await _conversationService.GetConversationsAsync()).ToList();
      ApplyFilter();
    }
    finally
    {
      IsLoading = false;
    }
  }

  partial void OnSearchTextChanged(string value) => ApplyFilter();

  partial void OnSelectedConversationChanged(ConversationListItem? value)
  {
    foreach (var conversation in Conversations)
      conversation.IsSelected = conversation == value;
  }

  [RelayCommand]
  private async Task OpenConversationAsync(ConversationListItem? conversation)
  {
    if (conversation is null)
      return;

    await Shell.Current.GoToAsync($"Chat?conversationId={conversation.Id}");
  }

  private void ApplyFilter()
  {
    var previousId = SelectedConversation?.Id;
    Conversations.Clear();

    var query = string.IsNullOrWhiteSpace(SearchText)
      ? _allConversations
      : _allConversations.Where(c =>
        c.DisplayName.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

    foreach (var conversation in query)
      Conversations.Add(conversation);

    SelectedConversation =
      Conversations.FirstOrDefault(c => c.Id == previousId)
      ?? Conversations.FirstOrDefault(c => c.IsSelected)
      ?? Conversations.FirstOrDefault();
  }

  private string GetUserInitials()
  {
    var session = _tokenService.GetSession();
    var name = session?.Username;

    if (string.IsNullOrWhiteSpace(name))
      return "JS";

    var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    return parts.Length >= 2
      ? $"{parts[0][0]}{parts[^1][0]}".ToUpperInvariant()
      : name.Length >= 2
        ? name[..2].ToUpperInvariant()
        : name.ToUpperInvariant();
  }
}
