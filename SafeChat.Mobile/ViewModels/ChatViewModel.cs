using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SafeChat.Mobile.DTO.SignalR;
using SafeChat.Mobile.Model;
using SafeChat.Mobile.Services.Api;
using SafeChat.Mobile.Services.Auth;
using SafeChat.Mobile.Services.RealTime;

namespace SafeChat.Mobile.ViewModels;

[QueryProperty(nameof(ConversationId), "conversationId")]
public partial class ChatViewModel : BaseViewModel, IDisposable
{
  private readonly ChatService _chatService;
  private readonly SignalRService _signalRService;
  private readonly TokenService _tokenService;
  private int _parsedConversationId;

  [ObservableProperty]
  private string _conversationId = string.Empty;

  [ObservableProperty]
  private string _contactName = string.Empty;

  [ObservableProperty]
  private string _contactInitials = string.Empty;

  [ObservableProperty]
  private bool _isOnline;

  [ObservableProperty]
  private string _presenceText = string.Empty;

  [ObservableProperty]
  private string _messageText = string.Empty;

  [ObservableProperty]
  private bool _isLoading;

  public ObservableCollection<ChatMessage> Messages { get; } = [];

  public ChatViewModel(ChatService chatService, SignalRService signalRService, TokenService tokenService)
  {
    _chatService = chatService;
    _signalRService = signalRService;
    _tokenService = tokenService;
    _signalRService.MessageReceived += OnMessageReceived;
  }

  partial void OnConversationIdChanged(string value)
  {
    if (int.TryParse(value, out _parsedConversationId))
      _ = LoadChatAsync();
  }

  [RelayCommand]
  private async Task LoadChatAsync()
  {
    if (_parsedConversationId <= 0 || IsLoading)
      return;

    IsLoading = true;

    try
    {
      var conversation = await _chatService.GetConversationAsync(_parsedConversationId);
      if (conversation is null)
        return;

      ContactName = conversation.DisplayName;
      ContactInitials = conversation.Initials;
      IsOnline = conversation.IsOnline;
      PresenceText = conversation.IsOnline ? "Online agora" : "Offline";

      Messages.Clear();
      var messages = await _chatService.GetMessagesAsync(_parsedConversationId);
      foreach (var message in messages)
        Messages.Add(message);

      await _signalRService.EnsureConnectedAsync();
      await _signalRService.JoinConversationAsync(_parsedConversationId);
    }
    finally
    {
      IsLoading = false;
    }
  }

  [RelayCommand]
  private async Task GoBackAsync()
  {
    await Shell.Current.GoToAsync("..");
  }

  [RelayCommand]
  private async Task SendMessage()
  {
    if (string.IsNullOrWhiteSpace(MessageText))
      return;

    var text = MessageText.Trim();
    MessageText = string.Empty;

    try
    {
      var (encryptedContent, encryptedAesKey, aesIv) = ChatService.EncodePayload(text);
      await _signalRService.SendMessageAsync(
        _parsedConversationId,
        encryptedContent,
        encryptedAesKey,
        aesIv);
    }
    catch (Exception ex)
    {
      MessageText = text;
      await Shell.Current.DisplayAlertAsync("Erro", ex.Message, "OK");
    }
  }

  private void OnMessageReceived(object? sender, NewMessageNotificationDto notification)
  {
    if (notification.ConversationId != _parsedConversationId)
      return;

    var currentUserId = _tokenService.GetSession()?.UserId ?? 0;
    var chatMessage = _chatService.MapToChatMessage(notification.Message, currentUserId);

    MainThread.BeginInvokeOnMainThread(() =>
    {
      if (Messages.Any(m => m.Id == chatMessage.Id))
        return;

      Messages.Add(chatMessage);
    });
  }

  public void Dispose()
  {
    _signalRService.MessageReceived -= OnMessageReceived;
  }
}
