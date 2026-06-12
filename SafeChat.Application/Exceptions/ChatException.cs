namespace SafeChat.Application.Exceptions;

public class ChatException : Exception
{
    public ChatException(string message) : base(message)
    {
    }
}
