using SafeChat.Mobile.Model;

namespace SafeChat.Mobile.Selectors;

public class ChatMessageTemplateSelector : DataTemplateSelector
{
  public DataTemplate? IncomingTemplate { get; set; }
  public DataTemplate? OutgoingTemplate { get; set; }
  public DataTemplate? SystemTemplate { get; set; }

  protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
  {
    if (item is ChatMessage message)
    {
      return message.Kind switch
      {
        ChatMessageKind.Outgoing => OutgoingTemplate ?? IncomingTemplate!,
        ChatMessageKind.System => SystemTemplate ?? IncomingTemplate!,
        _ => IncomingTemplate!
      };
    }

    return IncomingTemplate!;
  }
}
