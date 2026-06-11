namespace SafeChat.Mobile.Models.DTOs.Keys;

/// <summary>
/// DTO da chave pública RSA-2048 de um utilizador, obtida via API.
/// </summary>
public class PublicKeyDto
{
    /// <summary>Identificador da chave.</summary>
    public int Id { get; set; }

    /// <summary>Identificador do utilizador proprietário.</summary>
    public int UserId { get; set; }

    /// <summary>Valor da chave pública RSA-2048 em Base64.</summary>
    public string KeyValue { get; set; } = string.Empty;

    /// <summary>Data/hora UTC de criação da chave.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Data/hora UTC da última atualização, se aplicável.</summary>
    public DateTime? UpdatedAt { get; set; }
}
