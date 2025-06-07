
using System;
using System.Security.Claims;
using AuthService.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService.Services.Interfaces.IAuthorizationService _authService;

    public AuthController(AuthService.Services.Interfaces.IAuthorizationService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);
        return Ok(result);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request)
    {
        var result = await _authService.RefreshAsync(request);
        return Ok(result);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var nameIdentifierClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (nameIdentifierClaim == null)
        {
            return Unauthorized("User identifier claim not found.");
        }
        var userId = int.Parse(nameIdentifierClaim.Value);
        await _authService.LogoutAsync(userId);
        return NoContent();
    }

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public IActionResult Me()
    {
        var nameIdentifierClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        return Ok(new
        {
            Id = nameIdentifierClaim != null ? int.Parse(nameIdentifierClaim.Value) : 0,
            Username = User.Identity != null ? User.Identity.Name : null,
            CompanyId = User.FindFirst("company_id")?.Value
        });
    }
}
