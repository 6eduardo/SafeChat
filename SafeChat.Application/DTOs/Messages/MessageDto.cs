namespace SafeChat.Application.DTOs.Messages;

public class MessageDto
{
    public int Id { get; set; }
    public int ConversationId { get; set; }
    public int SenderId { get; set; }
    public string SenderUsername { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
    public string EncryptedContent { get; set; } = string.Empty;
    public string EncryptedAesKey { get; set; } = string.Empty;
    public string AesIv { get; set; } = string.Empty;
}
