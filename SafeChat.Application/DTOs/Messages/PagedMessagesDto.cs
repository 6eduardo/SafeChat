namespace SafeChat.Application.DTOs.Messages;

public class PagedMessagesDto
{
    public List<MessageDto> Items { get; set; } = [];
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}
