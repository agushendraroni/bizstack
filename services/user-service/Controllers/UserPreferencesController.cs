using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models;
using SharedLibrary.DTOs;

namespace UserService.Controllers;

[ApiController]
[ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
public class UserPreferencesController : ControllerBase
{
    private readonly UserDbContext _context;

    public UserPreferencesController(UserDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetPreferences()
    {
        var preferences = await _context.UserPreferences.ToListAsync();
        return Ok(new ApiResponse<List<UserPreference>> { Data = preferences, IsSuccess = true });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPreference(Guid id)
    {
        var preference = await _context.UserPreferences.FindAsync(id);
        if (preference == null)
            return NotFound(new ApiResponse<UserPreference> { Data = null, IsSuccess = false, Message = "Preference not found" });
        
        return Ok(new ApiResponse<UserPreference> { Data = preference, IsSuccess = true });
    }

    [HttpPost]
    public async Task<IActionResult> CreatePreference([FromBody] CreateUserPreferenceDto dto)
    {
        var preference = new UserPreference
        {
            UserId = dto.UserId,
            Language = dto.Language ?? "en",
            Theme = dto.Theme ?? "light",
            Timezone = dto.Timezone,
            ReceiveNotifications = dto.ReceiveNotifications
        };

        _context.UserPreferences.Add(preference);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetPreference), new { id = preference.Id }, new ApiResponse<UserPreference> { Data = preference, IsSuccess = true });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePreference(Guid id, [FromBody] UpdateUserPreferenceDto dto)
    {
        var preference = await _context.UserPreferences.FindAsync(id);
        if (preference == null)
            return NotFound(new ApiResponse<UserPreference> { Data = null, IsSuccess = false, Message = "Preference not found" });

        if (!string.IsNullOrEmpty(dto.Language)) preference.Language = dto.Language;
        if (!string.IsNullOrEmpty(dto.Theme)) preference.Theme = dto.Theme;
        if (!string.IsNullOrEmpty(dto.Timezone)) preference.Timezone = dto.Timezone;
        if (dto.ReceiveNotifications.HasValue) preference.ReceiveNotifications = dto.ReceiveNotifications.Value;

        await _context.SaveChangesAsync();
        return Ok(new ApiResponse<UserPreference> { Data = preference, IsSuccess = true });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePreference(Guid id)
    {
        var preference = await _context.UserPreferences.FindAsync(id);
        if (preference == null)
            return NotFound(new ApiResponse<UserPreference> { Data = null, IsSuccess = false, Message = "Preference not found" });

        _context.UserPreferences.Remove(preference);
        await _context.SaveChangesAsync();
        
        return Ok(new ApiResponse<string> { Data = "Preference deleted successfully", IsSuccess = true });
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

public class CreateUserPreferenceDto
{
    public Guid UserId { get; set; }
    public string? Language { get; set; }
    public string? Theme { get; set; }
    public string? Timezone { get; set; }
    public bool ReceiveNotifications { get; set; } = true;
}

public class UpdateUserPreferenceDto
{
    public string? Language { get; set; }
    public string? Theme { get; set; }
    public string? Timezone { get; set; }
    public bool? ReceiveNotifications { get; set; }
}