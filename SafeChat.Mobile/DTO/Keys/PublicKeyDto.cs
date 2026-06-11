namespace SafeChat.Mobile.DTO.Keys;

/// <summary>
/// Chave pública RSA de um utilizador devolvida pela API.
/// </summary>
public class PublicKeyDto
{
    /// <summary>Identificador da chave.</summary>
    public int Id { get; set; }

    /// <summary>Identificador do utilizador proprietário.</summary>
    public int UserId { get; set; }

    /// <summary>Valor da chave RSA-2048 em Base64.</summary>
    public string KeyValue { get; set; } = string.Empty;

    /// <summary>Data/hora UTC de criação.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Data/hora UTC da última atualização.</summary>
    public DateTime? UpdatedAt { get; set; }
}
