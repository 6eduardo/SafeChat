using SafeChat.Mobile.Configuration;
using SafeChat.Mobile.DTO.Contacts;
using SafeChat.Mobile.Services.Auth;
using UserContact = SafeChat.Mobile.Model.Contact;
using SafeChat.Mobile.Services.Interfaces;

namespace SafeChat.Mobile.Services.Api;

/// <summary>
/// Pesquisa de utilizadores via SafeChat.API.
/// </summary>
public class ContactService : BaseApiService, IContactService
{
    public ContactService(HttpClient httpClient, ApiConfiguration configuration, TokenService tokenService)
        : base(httpClient, configuration, tokenService)
    {
    }

    public async Task<IReadOnlyList<UserContact>> SearchUsersAsync(
        string query,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return [];

        var encodedQuery = Uri.EscapeDataString(query.Trim());
        var results = await GetAsync<List<UserSearchResultDto>>(
            $"api/users/search?q={encodedQuery}",
            cancellationToken);

        return results.Select(MapToContact).ToList();
    }

    private static UserContact MapToContact(UserSearchResultDto dto) =>
        new()
        {
            UserId = dto.UserId,
            Username = dto.Username,
            Email = dto.Email,
            PublicKey = dto.PublicKey,
            IsOnline = dto.IsOnline
        };
}
