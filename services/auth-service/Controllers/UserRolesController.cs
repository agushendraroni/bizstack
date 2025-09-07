using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp.Versioning;
using AuthService.Data;
using AuthService.Models;
using SharedLibrary.DTOs;

namespace AuthService.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UserRolesController : ControllerBase
{
    private readonly AuthDbContext _context;

    public UserRolesController(AuthDbContext context)
    {
        _context = context;
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserRoles(Guid userId)
    {
        var roles = await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Include(ur => ur.Role)
            .Select(ur => ur.Role)
            .ToListAsync();
        
        return Ok(new ApiResponse<List<Role>> { Data = roles, IsSuccess = true, Message = "User roles retrieved successfully" });
    }

    [HttpGet("role/{roleId}")]
    public async Task<IActionResult> GetRoleUsers(Guid roleId)
    {
        var users = await _context.UserRoles
            .Where(ur => ur.RoleId == roleId)
            .Include(ur => ur.User)
            .Select(ur => ur.User)
            .ToListAsync();
        
        return Ok(new ApiResponse<List<User>> { Data = users, IsSuccess = true, Message = "Role users retrieved successfully" });
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleDto dto)
    {
        var exists = await _context.UserRoles
            .AnyAsync(ur => ur.UserId == dto.UserId && ur.RoleId == dto.RoleId);
        
        if (exists)
            return BadRequest(new ApiResponse<string> { Data = null, IsSuccess = false, Message = "Role already assigned to user" });

        var userRole = new UserRole
        {
            UserId = dto.UserId,
            RoleId = dto.RoleId
        };

        _context.UserRoles.Add(userRole);
        await _context.SaveChangesAsync();
        
        return Ok(new ApiResponse<string> { Data = "Role assigned to user successfully", IsSuccess = true, Message = "Role assigned to user successfully" });
    }

    [HttpDelete("remove")]
    public async Task<IActionResult> RemoveRoleFromUser([FromBody] AssignRoleDto dto)
    {
        var userRole = await _context.UserRoles
            .FirstOrDefaultAsync(ur => ur.UserId == dto.UserId && ur.RoleId == dto.RoleId);
        
        if (userRole == null)
            return NotFound(new ApiResponse<string> { Data = null, IsSuccess = false, Message = "Role not assigned to user" });

        _context.UserRoles.Remove(userRole);
        await _context.SaveChangesAsync();
        
        return Ok(new ApiResponse<string> { Data = "Role removed from user successfully", IsSuccess = true, Message = "Role removed from user successfully" });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUserRoles()
    {
        var userRoles = await _context.UserRoles
            .Include(ur => ur.User)
            .Include(ur => ur.Role)
            .ToListAsync();
        
        return Ok(new ApiResponse<List<UserRole>> { Data = userRoles, IsSuccess = true, Message = "All user roles retrieved successfully" });
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

public class AssignRoleDto
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}
