using ArchitectureCatalog.API.DTOs.Auth;
using ArchitectureCatalog.API.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArchitectureCatalog.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        if (request is null)
        {
            return BadRequest();
        }

        var result = await _authService.LoginAsync(request);
        if (result is null)
        {
            return Unauthorized();
        }

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<LoginResponseDto>> Register([FromBody] RegisterRequestDto request)
    {
        if (request is null)
        {
            return BadRequest();
        }

        var (result, usernameTaken) = await _authService.RegisterAsync(request);
        if (usernameTaken)
        {
            return Conflict(new { message = "Username is already taken." });
        }

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult<LoginResponseDto>> Refresh([FromBody] RefreshTokenRequestDto request)
    {
        if (request is null)
        {
            return BadRequest();
        }

        var result = await _authService.RefreshAsync(request);
        if (result is null)
        {
            return Unauthorized();
        }

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto request)
    {
        if (request is null)
        {
            return BadRequest();
        }

        await _authService.LogoutAsync(request);
        return NoContent();
    }
}
