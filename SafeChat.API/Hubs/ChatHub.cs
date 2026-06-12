using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SafeChat.Application.DTOs.SignalR;
using SafeChat.Application.Exceptions;
using SafeChat.Application.Interfaces.Repositories;
using SafeChat.Application.Interfaces.Services;

namespace SafeChat.API.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IMessageService _messageService;
    private readonly IConversationRepository _conversationRepository;

    public ChatHub(IMessageService messageService, IConversationRepository conversationRepository)
    {
        _messageService = messageService;
        _conversationRepository = conversationRepository;
    }

    public async Task JoinConversation(int conversationId)
    {
        var userId = GetCurrentUserId();

        if (!await _conversationRepository.IsParticipantAsync(conversationId, userId))
            throw new HubException("Acesso negado a esta conversa.");

        await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(conversationId));
    }

    public async Task SendMessage(
        int conversationId,
        string encryptedContent,
        string encryptedAesKey,
        string aesIv)
    {
        var userId = GetCurrentUserId();

        try
        {
            var message = await _messageService.SendAsync(
                conversationId,
                userId,
                encryptedContent,
                encryptedAesKey,
                aesIv);

            var notification = new NewMessageNotificationDto
            {
                ConversationId = conversationId,
                Message = message
            };

            await Clients.Group(GetGroupName(conversationId))
                .SendAsync("ReceiveMessage", notification);
        }
        catch (ChatException ex)
        {
            throw new HubException(ex.Message);
        }
    }

    private int GetCurrentUserId()
    {
        var claim = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? Context.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (claim is null || !int.TryParse(claim, out var userId))
            throw new HubException("Utilizador não autenticado.");

        return userId;
    }

    private static string GetGroupName(int conversationId) => $"conversation-{conversationId}";
}
