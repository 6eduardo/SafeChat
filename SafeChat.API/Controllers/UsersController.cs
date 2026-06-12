using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeChat.Application.DTOs.Users;
using SafeChat.Application.Exceptions;
using SafeChat.Application.Interfaces.Services;

namespace SafeChat.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(IReadOnlyList<UserSearchResultDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] string q, CancellationToken cancellationToken)
    {
        var results = await _userService.SearchUsersAsync(GetCurrentUserId(), q, cancellationToken);
        return Ok(results);
    }

    [HttpGet("{userId:int}/publickey")]
    [ProducesResponseType(typeof(UserPublicKeyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPublicKey(int userId, CancellationToken cancellationToken)
    {
        try
        {
            var publicKey = await _userService.GetUserPublicKeyAsync(userId, cancellationToken);
            return Ok(publicKey);
        }
        catch (ChatException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    private int GetCurrentUserId()
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return int.Parse(claim!);
    }
}
