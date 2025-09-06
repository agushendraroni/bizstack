using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthService.Data;
using AuthService.Models;
using SharedLibrary.DTOs;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
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
        
        return Ok(ApiResponse<List<Role>>.Success(roles));
    }

    [HttpGet("role/{roleId}")]
    public async Task<IActionResult> GetRoleUsers(Guid roleId)
    {
        var users = await _context.UserRoles
            .Where(ur => ur.RoleId == roleId)
            .Include(ur => ur.User)
            .Select(ur => ur.User)
            .ToListAsync();
        
        return Ok(ApiResponse<List<User>>.Success(users));
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleDto dto)
    {
        var exists = await _context.UserRoles
            .AnyAsync(ur => ur.UserId == dto.UserId && ur.RoleId == dto.RoleId);
        
        if (exists)
            return BadRequest(ApiResponse<string>.Error("Role already assigned to user"));

        var userRole = new UserRole
        {
            UserId = dto.UserId,
            RoleId = dto.RoleId
        };

        _context.UserRoles.Add(userRole);
        await _context.SaveChangesAsync();
        
        return Ok(ApiResponse<string>.Success("Role assigned to user successfully"));
    }

    [HttpDelete("remove")]
    public async Task<IActionResult> RemoveRoleFromUser([FromBody] AssignRoleDto dto)
    {
        var userRole = await _context.UserRoles
            .FirstOrDefaultAsync(ur => ur.UserId == dto.UserId && ur.RoleId == dto.RoleId);
        
        if (userRole == null)
            return NotFound(ApiResponse<string>.Error("Role not assigned to user"));

        _context.UserRoles.Remove(userRole);
        await _context.SaveChangesAsync();
        
        return Ok(ApiResponse<string>.Success("Role removed from user successfully"));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUserRoles()
    {
        var userRoles = await _context.UserRoles
            .Include(ur => ur.User)
            .Include(ur => ur.Role)
            .ToListAsync();
        
        return Ok(ApiResponse<List<UserRole>>.Success(userRoles));
    }
}

public class AssignRoleDto
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}
