namespace SafeChat.Mobile.Model;

/// <summary>
/// Modelo de conversa para listagens e ecrã de chat na UI.
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
    public List<User> Participants { get; set; } = [];

    /// <summary>Pré-visualização da última mensagem (texto claro, após desencriptação local).</summary>
    public string LastMessagePreview { get; set; } = string.Empty;

    /// <summary>Título da conversa para exibição na UI (ex.: nome do contacto).</summary>
    public string DisplayTitle { get; set; } = string.Empty;
}
