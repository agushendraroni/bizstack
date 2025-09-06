using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models;
using SharedLibrary.DTOs;

namespace UserService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserActivityLogsController : ControllerBase
{
    private readonly UserDbContext _context;

    public UserActivityLogsController(UserDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetActivityLogs([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var logs = await _context.UserActivityLogs
            .OrderByDescending(log => log.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return Ok(ApiResponse<List<UserActivityLog>>.Success(logs));
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserActivityLogs(Guid userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var logs = await _context.UserActivityLogs
            .Where(log => log.UserId == userId)
            .OrderByDescending(log => log.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return Ok(ApiResponse<List<UserActivityLog>>.Success(logs));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetActivityLog(Guid id)
    {
        var log = await _context.UserActivityLogs.FindAsync(id);
        if (log == null)
            return NotFound(ApiResponse<UserActivityLog>.Error("Activity log not found"));
        
        return Ok(ApiResponse<UserActivityLog>.Success(log));
    }

    [HttpPost]
    public async Task<IActionResult> CreateActivityLog([FromBody] CreateActivityLogDto dto)
    {
        var log = new UserActivityLog
        {
            UserId = dto.UserId,
            Activity = dto.Activity,
            Description = dto.Description,
            IpAddress = dto.IpAddress,
            UserAgent = dto.UserAgent
        };

        _context.UserActivityLogs.Add(log);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetActivityLog), new { id = log.Id }, ApiResponse<UserActivityLog>.Success(log));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivityLog(Guid id)
    {
        var log = await _context.UserActivityLogs.FindAsync(id);
        if (log == null)
            return NotFound(ApiResponse<UserActivityLog>.Error("Activity log not found"));

        _context.UserActivityLogs.Remove(log);
        await _context.SaveChangesAsync();
        
        return Ok(ApiResponse<string>.Success("Activity log deleted successfully"));
    }

    [HttpGet("user/{userId}/activities/{activity}")]
    public async Task<IActionResult> GetUserActivityByAction(Guid userId, string activity)
    {
        var logs = await _context.UserActivityLogs
            .Where(log => log.UserId == userId && log.Activity == activity)
            .OrderByDescending(log => log.CreatedAt)
            .ToListAsync();
        
        return Ok(ApiResponse<List<UserActivityLog>>.Success(logs));
    }
}

public class CreateActivityLogDto
{
    public Guid UserId { get; set; }
    public string Activity { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
}
