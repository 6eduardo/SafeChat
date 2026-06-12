namespace SafeChat.Mobile.DTO.Conversations;

/// <summary>
/// Pedido para criar uma nova conversa entre utilizadores.
/// </summary>
public class CreateConversationRequestDto
{
    /// <summary>Identificador do outro participante (excluindo o utilizador autenticado).</summary>
    public int ParticipantUserId { get; set; }
}
