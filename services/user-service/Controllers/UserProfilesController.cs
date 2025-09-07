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
public class UserProfilesController : ControllerBase
{
    private readonly UserDbContext _context;

    public UserProfilesController(UserDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfiles()
    {
        var profiles = await _context.UserProfiles.ToListAsync();
        return Ok(new ApiResponse<List<UserProfile>> { Data = profiles, IsSuccess = true });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProfile(Guid id)
    {
        var profile = await _context.UserProfiles.FindAsync(id);
        if (profile == null)
            return NotFound(new ApiResponse<UserProfile> { Data = null, IsSuccess = false, Message = "Profile not found" });
        
        return Ok(new ApiResponse<UserProfile> { Data = profile, IsSuccess = true });
    }

    [HttpPost]
    public async Task<IActionResult> CreateProfile([FromBody] CreateUserProfileDto dto)
    {
        var profile = new UserProfile
        {
            UserId = dto.UserId,
            FullName = $"{dto.FirstName} {dto.LastName}".Trim(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            Address = dto.Address,
            City = dto.City,
            Country = dto.Country,
            PostalCode = dto.PostalCode,
            PhoneNumber = dto.PhoneNumber
        };

        _context.UserProfiles.Add(profile);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetProfile), new { id = profile.Id }, new ApiResponse<UserProfile> { Data = profile, IsSuccess = true });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProfile(Guid id, [FromBody] UpdateUserProfileDto dto)
    {
        var profile = await _context.UserProfiles.FindAsync(id);
        if (profile == null)
            return NotFound(new ApiResponse<UserProfile> { Data = null, IsSuccess = false, Message = "Profile not found" });

        if (!string.IsNullOrEmpty(dto.FirstName)) profile.FirstName = dto.FirstName;
        if (!string.IsNullOrEmpty(dto.LastName)) profile.LastName = dto.LastName;
        
        // Update FullName if FirstName or LastName changed
        if (!string.IsNullOrEmpty(dto.FirstName) || !string.IsNullOrEmpty(dto.LastName))
        {
            profile.FullName = $"{profile.FirstName} {profile.LastName}".Trim();
        }
        if (dto.DateOfBirth.HasValue) profile.DateOfBirth = dto.DateOfBirth;
        if (!string.IsNullOrEmpty(dto.Gender)) profile.Gender = dto.Gender;
        if (!string.IsNullOrEmpty(dto.Address)) profile.Address = dto.Address;
        if (!string.IsNullOrEmpty(dto.City)) profile.City = dto.City;
        if (!string.IsNullOrEmpty(dto.Country)) profile.Country = dto.Country;
        if (!string.IsNullOrEmpty(dto.PostalCode)) profile.PostalCode = dto.PostalCode;
        if (!string.IsNullOrEmpty(dto.PhoneNumber)) profile.PhoneNumber = dto.PhoneNumber;

        await _context.SaveChangesAsync();
        return Ok(new ApiResponse<UserProfile> { Data = profile, IsSuccess = true });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProfile(Guid id)
    {
        var profile = await _context.UserProfiles.FindAsync(id);
        if (profile == null)
            return NotFound(new ApiResponse<UserProfile> { Data = null, IsSuccess = false, Message = "Profile not found" });

        _context.UserProfiles.Remove(profile);
        await _context.SaveChangesAsync();
        
        return Ok(new ApiResponse<string> { Data = "Profile deleted successfully", IsSuccess = true });
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

public class CreateUserProfileDto
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? PhoneNumber { get; set; }
}

public class UpdateUserProfileDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? PhoneNumber { get; set; }
}