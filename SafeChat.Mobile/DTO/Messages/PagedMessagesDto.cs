namespace SafeChat.Mobile.DTO.Messages;

/// <summary>
/// Histórico paginado de mensagens encriptadas.
/// </summary>
public class PagedMessagesDto
{
    public List<MessageDto> Items { get; set; } = [];
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}
