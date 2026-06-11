namespace SafeChat.Mobile.DTO.Contacts;

/// <summary>
/// Resultado de pesquisa de utilizadores para adicionar como contacto.
/// </summary>
public class UserSearchResultDto
{
    /// <summary>Identificador do utilizador encontrado.</summary>
    public int UserId { get; set; }

    /// <summary>Nome de utilizador.</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>E-mail do utilizador.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Chave pública RSA-2048 em Base64.</summary>
    public string PublicKey { get; set; } = string.Empty;
}
