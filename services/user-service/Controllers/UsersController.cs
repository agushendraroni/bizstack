using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using UserService.DTOs;
using UserService.Services;

namespace UserService.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var tenantId = GetTenantId();
        var result = await _userService.GetAllUsersAsync(tenantId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var result = await _userService.GetUserByIdAsync(id);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet("username/{username}")]
    public async Task<IActionResult> GetUserByUsername(string username)
    {
        var result = await _userService.GetUserByUsernameAsync(username);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet("organization/{organizationId}")]
    public async Task<IActionResult> GetUsersByOrganization(Guid organizationId)
    {
        var result = await _userService.GetUsersByOrganizationAsync(organizationId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        var tenantId = GetTenantId();
        var userId = GetUserId();
        var result = await _userService.CreateUserAsync(createUserDto, tenantId, userId);
        return result.IsSuccess ? CreatedAtAction(nameof(GetUserById), new { id = result.Data?.Id }, result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto updateUserDto)
    {
        var tenantId = GetTenantId();
        var userId = GetUserId();
        var result = await _userService.UpdateUserAsync(id, updateUserDto, tenantId, userId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var tenantId = GetTenantId();
        var userId = GetUserId();
        var result = await _userService.DeleteUserAsync(id, tenantId, userId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    private int? GetTenantId()
    {
        if (Request.Headers.TryGetValue("X-Tenant-Id", out var tenantIdHeader) && 
            int.TryParse(tenantIdHeader.FirstOrDefault(), out var tenantId))
        {
            return tenantId;
        }
        return null;
    }

    private Guid? GetUserId()
    {
        if (Request.Headers.TryGetValue("X-User-Id", out var userIdHeader) && 
            Guid.TryParse(userIdHeader.FirstOrDefault(), out var userId))
        {
            return userId;
        }
        return null;
    }
}
