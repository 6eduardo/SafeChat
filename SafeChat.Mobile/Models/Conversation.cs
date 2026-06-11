namespace SafeChat.Mobile.Models;

/// <summary>
/// Modelo local de conversa para listagens e navegação no chat.
/// </summary>
public class Conversation
{
    /// <summary>Identificador da conversa.</summary>
    public int Id { get; set; }

    /// <summary>Data/hora UTC de criação.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Data/hora UTC da última mensagem.</summary>
    public DateTime? LastMessageAt { get; set; }

    /// <summary>Participantes da conversa.</summary>
    public List<Contact> Participants { get; set; } = [];

    /// <summary>Pré-visualização da última mensagem (texto claro, após desencriptação local).</summary>
    public string LastMessagePreview { get; set; } = string.Empty;

    /// <summary>Título apresentado na UI (nome do outro participante em conversas 1:1).</summary>
    public string DisplayTitle { get; set; } = string.Empty;
}
