using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models;
using SharedLibrary.DTOs;

namespace UserService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserProfilesController : ControllerBase
{
    private readonly UserDbContext _context;

    public UserProfilesController(UserDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserProfiles()
    {
        var profiles = await _context.UserProfiles.ToListAsync();
        return Ok(ApiResponse<List<UserProfile>>.Success(profiles));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserProfile(Guid id)
    {
        var profile = await _context.UserProfiles.FindAsync(id);
        if (profile == null)
            return NotFound(ApiResponse<UserProfile>.Error("User profile not found"));
        
        return Ok(ApiResponse<UserProfile>.Success(profile));
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserProfileByUserId(Guid userId)
    {
        var profile = await _context.UserProfiles
            .FirstOrDefaultAsync(up => up.UserId == userId);
        
        if (profile == null)
            return NotFound(ApiResponse<UserProfile>.Error("User profile not found"));
        
        return Ok(ApiResponse<UserProfile>.Success(profile));
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserProfile([FromBody] CreateUserProfileDto dto)
    {
        var profile = new UserProfile
        {
            UserId = dto.UserId,
            FullName = dto.FullName,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            AvatarUrl = dto.AvatarUrl,
            Bio = dto.Bio
        };

        _context.UserProfiles.Add(profile);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetUserProfile), new { id = profile.Id }, ApiResponse<UserProfile>.Success(profile));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserProfile(Guid id, [FromBody] UpdateUserProfileDto dto)
    {
        var profile = await _context.UserProfiles.FindAsync(id);
        if (profile == null)
            return NotFound(ApiResponse<UserProfile>.Error("User profile not found"));

        if (!string.IsNullOrEmpty(dto.FullName)) profile.FullName = dto.FullName;
        if (!string.IsNullOrEmpty(dto.PhoneNumber)) profile.PhoneNumber = dto.PhoneNumber;
        if (!string.IsNullOrEmpty(dto.Email)) profile.Email = dto.Email;
        if (dto.DateOfBirth.HasValue) profile.DateOfBirth = dto.DateOfBirth;
        if (!string.IsNullOrEmpty(dto.Gender)) profile.Gender = dto.Gender;
        if (!string.IsNullOrEmpty(dto.AvatarUrl)) profile.AvatarUrl = dto.AvatarUrl;
        if (!string.IsNullOrEmpty(dto.Bio)) profile.Bio = dto.Bio;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<UserProfile>.Success(profile));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserProfile(Guid id)
    {
        var profile = await _context.UserProfiles.FindAsync(id);
        if (profile == null)
            return NotFound(ApiResponse<UserProfile>.Error("User profile not found"));

        _context.UserProfiles.Remove(profile);
        await _context.SaveChangesAsync();
        
        return Ok(ApiResponse<string>.Success("User profile deleted successfully"));
    }
}

public class CreateUserProfileDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }
}

public class UpdateUserProfileDto
{
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }
}
