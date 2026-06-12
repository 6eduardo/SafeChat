using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeChat.Application.DTOs.Conversations;
using SafeChat.Application.Exceptions;
using SafeChat.Application.Interfaces.Services;

namespace SafeChat.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ConversationsController : ControllerBase
{
    private readonly IConversationService _conversationService;

    public ConversationsController(IConversationService conversationService)
    {
        _conversationService = conversationService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ConversationSummaryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateConversationRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _conversationService.CreateAsync(
                GetCurrentUserId(),
                request.ParticipantUserId,
                cancellationToken);

            return CreatedAtAction(nameof(GetAll), new { id = response.Id }, response);
        }
        catch (ChatException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ConversationSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var conversations = await _conversationService.GetUserConversationsAsync(
            GetCurrentUserId(),
            cancellationToken);

        return Ok(conversations);
    }

    private int GetCurrentUserId()
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return int.Parse(claim!);
    }
}
