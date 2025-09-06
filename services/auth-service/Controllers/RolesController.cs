using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthService.Data;
using AuthService.Models;
using SharedLibrary.DTOs;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly AuthDbContext _context;

    public RolesController(AuthDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _context.Roles.ToListAsync();
        return Ok(ApiResponse<List<Role>>.Success(roles));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRole(Guid id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null)
            return NotFound(ApiResponse<Role>.Error("Role not found"));
        
        return Ok(ApiResponse<Role>.Success(role));
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto dto)
    {
        var role = new Role
        {
            Name = dto.Name,
            Description = dto.Description
        };

        _context.Roles.Add(role);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetRole), new { id = role.Id }, ApiResponse<Role>.Success(role));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRole(Guid id, [FromBody] UpdateRoleDto dto)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null)
            return NotFound(ApiResponse<Role>.Error("Role not found"));

        if (!string.IsNullOrEmpty(dto.Name)) role.Name = dto.Name;
        if (!string.IsNullOrEmpty(dto.Description)) role.Description = dto.Description;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<Role>.Success(role));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(Guid id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null)
            return NotFound(ApiResponse<Role>.Error("Role not found"));

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();
        
        return Ok(ApiResponse<string>.Success("Role deleted successfully"));
    }
}

public class CreateRoleDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class UpdateRoleDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
