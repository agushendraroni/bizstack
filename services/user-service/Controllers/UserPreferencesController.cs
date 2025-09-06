using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models;
using SharedLibrary.DTOs;

namespace UserService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserPreferencesController : ControllerBase
{
    private readonly UserDbContext _context;

    public UserPreferencesController(UserDbContext context)
    {
        _context = context;
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserPreferences(Guid userId)
    {
        var preferences = await _context.UserPreferences
            .Where(up => up.UserId == userId)
            .ToListAsync();
        
        return Ok(ApiResponse<List<UserPreference>>.Success(preferences));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserPreference(Guid id)
    {
        var preference = await _context.UserPreferences.FindAsync(id);
        if (preference == null)
            return NotFound(ApiResponse<UserPreference>.Error("User preference not found"));
        
        return Ok(ApiResponse<UserPreference>.Success(preference));
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserPreference([FromBody] CreateUserPreferenceDto dto)
    {
        var preference = new UserPreference
        {
            UserId = dto.UserId,
            Language = dto.Language,
            Theme = dto.Theme,
            Timezone = dto.Timezone,
            ReceiveNotifications = dto.ReceiveNotifications
        };

        _context.UserPreferences.Add(preference);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetUserPreference), new { id = preference.Id }, ApiResponse<UserPreference>.Success(preference));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserPreference(Guid id, [FromBody] UpdateUserPreferenceDto dto)
    {
        var preference = await _context.UserPreferences.FindAsync(id);
        if (preference == null)
            return NotFound(ApiResponse<UserPreference>.Error("User preference not found"));

        if (!string.IsNullOrEmpty(dto.Language)) preference.Language = dto.Language;
        if (!string.IsNullOrEmpty(dto.Theme)) preference.Theme = dto.Theme;
        if (!string.IsNullOrEmpty(dto.Timezone)) preference.Timezone = dto.Timezone;
        if (dto.ReceiveNotifications.HasValue) preference.ReceiveNotifications = dto.ReceiveNotifications.Value;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<UserPreference>.Success(preference));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserPreference(Guid id)
    {
        var preference = await _context.UserPreferences.FindAsync(id);
        if (preference == null)
            return NotFound(ApiResponse<UserPreference>.Error("User preference not found"));

        _context.UserPreferences.Remove(preference);
        await _context.SaveChangesAsync();
        
        return Ok(ApiResponse<string>.Success("User preference deleted successfully"));
    }

    [HttpGet("user/{userId}/language/{language}")]
    public async Task<IActionResult> GetUserPreferenceByLanguage(Guid userId, string language)
    {
        var preference = await _context.UserPreferences
            .FirstOrDefaultAsync(up => up.UserId == userId && up.Language == language);
        
        if (preference == null)
            return NotFound(ApiResponse<UserPreference>.Error("User preference not found"));
        
        return Ok(ApiResponse<UserPreference>.Success(preference));
    }
}

public class CreateUserPreferenceDto
{
    public Guid UserId { get; set; }
    public string Language { get; set; } = "en";
    public string Theme { get; set; } = "light";
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
