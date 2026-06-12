using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SafeChat.Mobile.Services.Auth;
using UserContact = SafeChat.Mobile.Model.Contact;
using SafeChat.Mobile.Services.Interfaces;

namespace SafeChat.Mobile.ViewModels;

public partial class ChatsViewModel : BaseViewModel
{
    private const int MinSearchLength = 2;

    private readonly IContactService _contactService;
    private readonly IConversationService _conversationService;
    private readonly TokenService _tokenService;
    private CancellationTokenSource? _searchCts;

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private string _userInitials = "SC";

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _hasSearched;

    [ObservableProperty]
    private string _emptyStateText = "Pesquisa por nome de utilizador ou e-mail para iniciar uma conversa.";

    public ObservableCollection<UserContact> Users { get; } = [];

    public bool HasResults => Users.Count > 0;

    public bool ShowEmptyState => !IsLoading && Users.Count == 0;

    public ChatsViewModel(
        IContactService contactService,
        IConversationService conversationService,
        TokenService tokenService)
    {
        _contactService = contactService;
        _conversationService = conversationService;
        _tokenService = tokenService;
        Users.CollectionChanged += (_, _) => RefreshListState();
    }

    partial void OnIsLoadingChanged(bool value) => RefreshListState();

    partial void OnSearchTextChanged(string value) => _ = DebouncedSearchAsync(value);

    [RelayCommand]
    private void RefreshHeader() => UserInitials = GetUserInitials();

    [RelayCommand]
    private async Task StartConversationAsync(UserContact? contact)
    {
        if (contact is null || IsLoading)
            return;

        IsLoading = true;

        try
        {
            var conversation = await _conversationService.CreateConversationAsync(contact.UserId);
            await Shell.Current.GoToAsync($"Chat?conversationId={conversation.Id}");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Erro", ex.Message, "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task DebouncedSearchAsync(string query)
    {
        _searchCts?.Cancel();
        _searchCts?.Dispose();
        _searchCts = new CancellationTokenSource();
        var token = _searchCts.Token;

        UserInitials = GetUserInitials();

        if (string.IsNullOrWhiteSpace(query) || query.Trim().Length < MinSearchLength)
        {
            Users.Clear();
            HasSearched = false;
            EmptyStateText = "Pesquisa por nome de utilizador ou e-mail para iniciar uma conversa.";
            RefreshListState();
            return;
        }

        try
        {
            await Task.Delay(400, token);
        }
        catch (TaskCanceledException)
        {
            return;
        }

        if (token.IsCancellationRequested)
            return;

        await SearchUsersAsync(query.Trim(), token);
    }

    private async Task SearchUsersAsync(string query, CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            var results = await _contactService.SearchUsersAsync(query, cancellationToken);

            Users.Clear();
            foreach (var user in results)
                Users.Add(user);

            HasSearched = true;
            EmptyStateText = Users.Count == 0
                ? "Nenhum utilizador encontrado."
                : string.Empty;
        }
        catch (Exception ex)
        {
            Users.Clear();
            HasSearched = true;
            EmptyStateText = ex.Message;
        }
        finally
        {
            IsLoading = false;
            RefreshListState();
        }
    }

    private void RefreshListState()
    {
        OnPropertyChanged(nameof(HasResults));
        OnPropertyChanged(nameof(ShowEmptyState));
    }

    private string GetUserInitials()
    {
        var session = _tokenService.GetSession();
        var name = session?.Username;

        if (string.IsNullOrWhiteSpace(name))
            return "SC";

        var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length >= 2
            ? $"{parts[0][0]}{parts[^1][0]}".ToUpperInvariant()
            : name.Length >= 2
                ? name[..2].ToUpperInvariant()
                : name.ToUpperInvariant();
    }
}
