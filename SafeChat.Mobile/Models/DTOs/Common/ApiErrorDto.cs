namespace SafeChat.Mobile.Models.DTOs.Common;

/// <summary>
/// DTO de erro devolvido pela API em respostas 4xx/5xx.
/// </summary>
public class ApiErrorDto
{
    /// <summary>Mensagem de erro legível para o utilizador.</summary>
    public string Message { get; set; } = string.Empty;
}
