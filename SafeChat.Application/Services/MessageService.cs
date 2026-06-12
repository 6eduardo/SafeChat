using SafeChat.Application.DTOs.Messages;
using SafeChat.Application.Exceptions;
using SafeChat.Application.Interfaces.Repositories;
using SafeChat.Application.Interfaces.Services;
using SafeChat.Domain.Entities;

namespace SafeChat.Application.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IConversationRepository _conversationRepository;

    public MessageService(
        IMessageRepository messageRepository,
        IConversationRepository conversationRepository)
    {
        _messageRepository = messageRepository;
        _conversationRepository = conversationRepository;
    }

    public async Task<MessageDto> SendAsync(
        int conversationId,
        int senderId,
        string encryptedContent,
        string encryptedAesKey,
        string aesIv,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(encryptedContent)
            || string.IsNullOrWhiteSpace(encryptedAesKey)
            || string.IsNullOrWhiteSpace(aesIv))
        {
            throw new ChatException("Payload encriptado inválido.");
        }

        if (!await _conversationRepository.IsParticipantAsync(conversationId, senderId, cancellationToken))
            throw new ChatException("Acesso negado a esta conversa.");

        var sentAt = DateTime.UtcNow;
        var message = new Message
        {
            ConversationId = conversationId,
            SenderId = senderId,
            SentAt = sentAt,
            EncryptedContent = encryptedContent.Trim(),
            EncryptedAesKey = encryptedAesKey.Trim(),
            AesIv = aesIv.Trim()
        };

        var created = await _messageRepository.AddAsync(message, cancellationToken);
        await _messageRepository.UpdateConversationLastMessageAtAsync(conversationId, sentAt, cancellationToken);

        return MapToDto(created);
    }

    public async Task<PagedMessagesDto> GetConversationMessagesAsync(
        int conversationId,
        int userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        if (!await _conversationRepository.IsParticipantAsync(conversationId, userId, cancellationToken))
            throw new ChatException("Acesso negado a esta conversa.");

        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var (items, totalCount) = await _messageRepository.GetByConversationAsync(
            conversationId, page, pageSize, cancellationToken);

        return new PagedMessagesDto
        {
            Items = items.Select(MapToDto).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private static MessageDto MapToDto(Message message) =>
        new()
        {
            Id = message.Id,
            ConversationId = message.ConversationId,
            SenderId = message.SenderId,
            SenderUsername = message.Sender?.Username ?? string.Empty,
            SentAt = message.SentAt,
            EncryptedContent = message.EncryptedContent,
            EncryptedAesKey = message.EncryptedAesKey,
            AesIv = message.AesIv
        };
}
