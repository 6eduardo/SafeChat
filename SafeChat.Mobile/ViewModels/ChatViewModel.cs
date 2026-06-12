using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SafeChat.Mobile.DTO.Messages;
using SafeChat.Mobile.DTO.SignalR;
using SafeChat.Mobile.Model;
using SafeChat.Mobile.Services.Api;
using SafeChat.Mobile.Services.Auth;
using SafeChat.Mobile.Services.Crypto;
using SafeChat.Mobile.Services.RealTime;
using SafeChat.Mobile.Services.Storage;

namespace SafeChat.Mobile.ViewModels;

[QueryProperty(nameof(ConversationId), "conversationId")]
public partial class ChatViewModel : BaseViewModel, IDisposable
{
  private readonly ChatService _chatService;
  private readonly SignalRService _signalRService;
  private readonly TokenService _tokenService;
  private readonly MessageEncryptionService _messageEncryptionService;
  private readonly SecureKeyStorageService _secureKeyStorageService;
  private int _parsedConversationId;
  private int _recipientUserId;
  private int _optimisticMessageId = -1;

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

  public ChatViewModel(
    ChatService chatService,
    SignalRService signalRService,
    TokenService tokenService,
    MessageEncryptionService messageEncryptionService,
    SecureKeyStorageService secureKeyStorageService)
  {
    _chatService = chatService;
    _signalRService = signalRService;
    _tokenService = tokenService;
    _messageEncryptionService = messageEncryptionService;
    _secureKeyStorageService = secureKeyStorageService;
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

      _recipientUserId = conversation.OtherParticipantUserId;
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

    if (_recipientUserId <= 0)
    {
      await Shell.Current.DisplayAlertAsync("Erro", "Destinatário da conversa não identificado.", "OK");
      return;
    }

    var text = MessageText.Trim();
    MessageText = string.Empty;

    try
    {
      var recipientPublicKey = await _chatService.GetUserPublicKeyAsync(_recipientUserId);
      var payload = _messageEncryptionService.EncryptMessage(text, recipientPublicKey);

      var optimisticMessage = _chatService.BuildOutgoingMessage(
        text,
        _parsedConversationId,
        _optimisticMessageId--);
      Messages.Add(optimisticMessage);

      await _signalRService.SendMessageAsync(
        _parsedConversationId,
        payload.EncryptedContent,
        payload.EncryptedAesKey,
        payload.AesIv);
    }
    catch (Exception ex)
    {
      MessageText = text;
      var last = Messages.LastOrDefault(m => m.Id < 0 && m.Content == text);
      if (last is not null)
        Messages.Remove(last);

      await Shell.Current.DisplayAlertAsync("Erro", ex.Message, "OK");
    }
  }

  private void OnMessageReceived(object? sender, NewMessageNotificationDto notification)
  {
    if (notification.ConversationId != _parsedConversationId)
      return;

    _ = HandleIncomingMessageAsync(notification);
  }

  private async Task HandleIncomingMessageAsync(NewMessageNotificationDto notification)
  {
    var currentUserId = _tokenService.GetSession()?.UserId ?? 0;

    if (notification.Message.SenderId == currentUserId)
    {
      MainThread.BeginInvokeOnMainThread(() =>
      {
        if (Messages.Any(m => m.Id == notification.Message.Id))
          return;

        var optimistic = Messages.LastOrDefault(m => m.Id < 0 && m.Kind == ChatMessageKind.Outgoing);
        if (optimistic is not null)
        {
          optimistic.Id = notification.Message.Id;
          optimistic.SentAt = notification.Message.SentAt;
          optimistic.TimestampDisplay = notification.Message.SentAt.ToLocalTime().ToString("HH:mm");
          return;
        }
      });

      return;
    }

    var privateKey = await _secureKeyStorageService.GetPrivateKeyAsync();
    if (string.IsNullOrEmpty(privateKey))
      return;

    ChatMessage chatMessage;
    try
    {
      var plaintext = _messageEncryptionService.DecryptMessage(
        notification.Message.EncryptedContent,
        notification.Message.EncryptedAesKey,
        notification.Message.AesIv,
        privateKey);

      chatMessage = BuildIncomingChatMessage(notification.Message, plaintext);
    }
    catch
    {
      chatMessage = BuildIncomingChatMessage(notification.Message, "Não foi possível desencriptar");
    }

    MainThread.BeginInvokeOnMainThread(() =>
    {
      if (Messages.Any(m => m.Id == chatMessage.Id))
        return;

      Messages.Add(chatMessage);
    });
  }

  private static ChatMessage BuildIncomingChatMessage(MessageDto dto, string content)
  {
    var initials = GetInitials(dto.SenderUsername);

    return new ChatMessage
    {
      Id = dto.Id,
      ConversationId = dto.ConversationId,
      SenderId = dto.SenderId,
      SenderUsername = dto.SenderUsername,
      SenderInitials = initials,
      Kind = ChatMessageKind.Incoming,
      Content = content,
      SentAt = dto.SentAt,
      TimestampDisplay = dto.SentAt.ToLocalTime().ToString("HH:mm")
    };
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

  public void Dispose()
  {
    _signalRService.MessageReceived -= OnMessageReceived;
  }
}
