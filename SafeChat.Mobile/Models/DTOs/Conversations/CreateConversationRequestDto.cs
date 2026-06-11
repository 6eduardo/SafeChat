namespace SafeChat.Mobile.Models.DTOs.Conversations;

/// <summary>
/// DTO de pedido para criar uma nova conversa entre utilizadores.
/// </summary>
public class CreateConversationRequestDto
{
    /// <summary>Identificadores dos utilizadores participantes (excluindo o utilizador autenticado).</summary>
    public List<int> ParticipantUserIds { get; set; } = [];
}
