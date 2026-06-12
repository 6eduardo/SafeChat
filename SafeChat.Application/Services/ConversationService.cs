using SafeChat.Application.DTOs.Conversations;
using SafeChat.Application.Exceptions;
using SafeChat.Application.Interfaces.Repositories;
using SafeChat.Application.Interfaces.Services;
using SafeChat.Domain.Entities;

namespace SafeChat.Application.Services;

public class ConversationService : IConversationService
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IUserRepository _userRepository;

    public ConversationService(
        IConversationRepository conversationRepository,
        IUserRepository userRepository)
    {
        _conversationRepository = conversationRepository;
        _userRepository = userRepository;
    }

    public async Task<ConversationSummaryDto> CreateAsync(
        int currentUserId,
        int participantUserId,
        CancellationToken cancellationToken = default)
    {
        if (participantUserId == currentUserId)
            throw new ChatException("Não é possível criar uma conversa consigo próprio.");

        var participant = await _userRepository.GetByIdAsync(participantUserId, cancellationToken);
        if (participant is null)
            throw new ChatException("Utilizador não encontrado.");

        var existing = await _conversationRepository.FindDirectConversationAsync(
            currentUserId, participantUserId, cancellationToken);

        var conversation = existing
            ?? await _conversationRepository.CreateDirectConversationAsync(
                currentUserId, participantUserId, cancellationToken);

        return MapToSummary(conversation, currentUserId);
    }

    public async Task<IReadOnlyList<ConversationSummaryDto>> GetUserConversationsAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        var conversations = await _conversationRepository.GetUserConversationsAsync(userId, cancellationToken);
        return conversations.Select(c => MapToSummary(c, userId)).ToList();
    }

    private static ConversationSummaryDto MapToSummary(Conversation conversation, int currentUserId)
    {
        var other = conversation.Participants
            .Select(p => p.User)
            .FirstOrDefault(u => u.Id != currentUserId);

        var lastMessage = conversation.Messages
            .OrderByDescending(m => m.SentAt)
            .FirstOrDefault();

        return new ConversationSummaryDto
        {
            Id = conversation.Id,
            CreatedAt = conversation.CreatedAt,
            LastMessageAt = conversation.LastMessageAt ?? lastMessage?.SentAt,
            DisplayName = other?.Username ?? "Conversa",
            Initials = GetInitials(other?.Username ?? "??"),
            OtherParticipantUserId = other?.Id ?? 0,
            IsOtherParticipantOnline = other?.IsOnline ?? false,
            LastMessagePreview = lastMessage is null ? "Sem mensagens" : "Mensagem encriptada"
        };
    }

    private static string GetInitials(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return "??";

        var parts = username.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length >= 2
            ? $"{parts[0][0]}{parts[^1][0]}".ToUpperInvariant()
            : username.Length >= 2
                ? username[..2].ToUpperInvariant()
                : username.ToUpperInvariant();
    }
}
