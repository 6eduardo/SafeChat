namespace SafeChat.Mobile.Model;

/// <summary>
/// Mensagem de chat para exibição na UI, com conteúdo já desencriptado localmente.
/// </summary>
public class ChatMessage
{
  /// <summary>Identificador da mensagem.</summary>
  public int Id { get; set; }

  /// <summary>Identificador da conversa.</summary>
  public int ConversationId { get; set; }

  /// <summary>Identificador do remetente.</summary>
  public int SenderId { get; set; }

  /// <summary>Nome de utilizador do remetente.</summary>
  public string SenderUsername { get; set; } = string.Empty;

  /// <summary>Iniciais do remetente para o avatar.</summary>
  public string SenderInitials { get; set; } = string.Empty;

  /// <summary>Conteúdo em texto claro (desencriptado no dispositivo).</summary>
  public string Content { get; set; } = string.Empty;

  /// <summary>Data/hora UTC de envio.</summary>
  public DateTime SentAt { get; set; }

  /// <summary>Tipo visual da mensagem na lista.</summary>
  public ChatMessageKind Kind { get; set; }

  /// <summary>Indica se a mensagem foi enviada pelo utilizador autenticado.</summary>
  public bool IsMine => Kind == ChatMessageKind.Outgoing;

  /// <summary>Hora formatada para exibição na UI.</summary>
  public string TimestampDisplay { get; set; } = string.Empty;

  public bool IsIncoming => Kind == ChatMessageKind.Incoming;
  public bool IsOutgoing => Kind == ChatMessageKind.Outgoing;
  public bool IsSystem => Kind == ChatMessageKind.System;
}
