using SafeChat.Mobile.Model;

namespace SafeChat.Mobile.Services.Interfaces;

/// <summary>
/// Contrato para obtenção do perfil do utilizador (API real ou mock).
/// </summary>
public interface IProfileService
{
  Task<UserProfile> GetProfileAsync(CancellationToken cancellationToken = default);
}
