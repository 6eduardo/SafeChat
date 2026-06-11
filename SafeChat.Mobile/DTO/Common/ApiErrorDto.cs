namespace SafeChat.Mobile.DTO.Common;

/// <summary>
/// Resposta de erro genérica devolvida pela API.
/// </summary>
public class ApiErrorDto
{
    /// <summary>Mensagem de erro legível.</summary>
    public string Message { get; set; } = string.Empty;
}
