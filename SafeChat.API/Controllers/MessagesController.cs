using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeChat.Application.DTOs.Messages;
using SafeChat.Application.Exceptions;
using SafeChat.Application.Interfaces.Services;

namespace SafeChat.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessagesController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpGet("{conversationId:int}")]
    [ProducesResponseType(typeof(PagedMessagesDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetByConversation(
        int conversationId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var messages = await _messageService.GetConversationMessagesAsync(
                conversationId,
                GetCurrentUserId(),
                page,
                pageSize,
                cancellationToken);

            return Ok(messages);
        }
        catch (ChatException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private int GetCurrentUserId()
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return int.Parse(claim!);
    }
}
