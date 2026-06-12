using UserContact = SafeChat.Mobile.Model.Contact;

namespace SafeChat.Mobile.Services.Interfaces;

/// <summary>
/// Contrato para pesquisa de utilizadores e contactos.
/// </summary>
public interface IContactService
{
    Task<IReadOnlyList<UserContact>> SearchUsersAsync(string query, CancellationToken cancellationToken = default);
}
