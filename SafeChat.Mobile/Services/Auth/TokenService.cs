using SafeChat.Mobile.Model;

namespace SafeChat.Mobile.Services.Auth;

public class TokenService
{
    public event Action? SessionCleared;

    private UserSession? _session;

    private static readonly string TokenKey = "auth_token";
    private static readonly string UserIdKey = "auth_user_id";
    private static readonly string UsernameKey = "auth_username";
    private static readonly string EmailKey = "auth_email";
    private static readonly string ExpiresAtKey = "auth_expires_at";

    public UserSession? GetSession()
    {
        return _session;
    }

    public void SaveSession(UserSession session)
    {
        _session = session;
        if (Preferences.ContainsKey(TokenKey))
            Preferences.Remove(TokenKey);
        Preferences.Set(TokenKey, session.Token);
        Preferences.Set(UserIdKey, session.UserId);
        Preferences.Set(UsernameKey, session.Username);
        Preferences.Set(EmailKey, session.Email);
        Preferences.Set(ExpiresAtKey, session.ExpiresAt.ToString("O"));
    }

    public void ClearSession()
    {
        _session = null;
        Preferences.Remove(TokenKey);
        Preferences.Remove(UserIdKey);
        Preferences.Remove(UsernameKey);
        Preferences.Remove(EmailKey);
        Preferences.Remove(ExpiresAtKey);
        SessionCleared?.Invoke();
    }

    public bool IsAuthenticated
    {
        get
        {
            if (_session is not null)
                return _session.IsValid;

            return TryRestoreSession();
        }
    }

    public string? Token => _session?.Token;

    private bool TryRestoreSession()
    {
        var token = Preferences.Get(TokenKey, string.Empty);
        if (string.IsNullOrEmpty(token))
            return false;

        var expiresAtStr = Preferences.Get(ExpiresAtKey, string.Empty);
        if (string.IsNullOrEmpty(expiresAtStr))
            return false;

        if (!DateTime.TryParse(expiresAtStr, null, System.Globalization.DateTimeStyles.RoundtripKind, out var expiresAt))
            return false;

        if (expiresAt <= DateTime.UtcNow)
            return false;

        _session = new UserSession
        {
            Token = token,
            UserId = Preferences.Get(UserIdKey, 0),
            Username = Preferences.Get(UsernameKey, string.Empty),
            Email = Preferences.Get(EmailKey, string.Empty),
            ExpiresAt = expiresAt
        };

        return _session.IsValid;
    }
}
