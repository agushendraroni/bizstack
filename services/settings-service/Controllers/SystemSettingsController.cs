using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SettingsService.Data;
using SettingsService.Models;
using SharedLibrary.DTOs;

namespace SettingsService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SystemSettingsController : ControllerBase
{
    private readonly SettingsDbContext _context;

    public SystemSettingsController(SettingsDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetSettings([FromQuery] string? category = null)
    {
        var query = _context.SystemSettings.AsQueryable();
        
        if (!string.IsNullOrEmpty(category))
            query = query.Where(s => s.Category == category);

        var settings = await query
            .OrderBy(s => s.Category)
            .ThenBy(s => s.SortOrder)
            .ThenBy(s => s.Key)
            .ToListAsync();

        return Ok(ApiResponse<List<SystemSetting>>.Success(settings));
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _context.SystemSettings
            .GroupBy(s => s.Category)
            .Select(g => new { Category = g.Key, Count = g.Count() })
            .OrderBy(c => c.Category)
            .ToListAsync();

        return Ok(ApiResponse<object>.Success(categories));
    }

    [HttpGet("key/{key}")]
    public async Task<IActionResult> GetSettingByKey(string key)
    {
        var setting = await _context.SystemSettings
            .FirstOrDefaultAsync(s => s.Key == key);

        if (setting == null)
            return NotFound(ApiResponse<SystemSetting>.Error("Setting not found"));

        return Ok(ApiResponse<SystemSetting>.Success(setting));
    }

    [HttpGet("value/{key}")]
    public async Task<IActionResult> GetSettingValue(string key)
    {
        var setting = await _context.SystemSettings
            .FirstOrDefaultAsync(s => s.Key == key);

        if (setting == null)
            return NotFound(ApiResponse<string>.Error("Setting not found"));

        var value = setting.Value ?? setting.DefaultValue;
        return Ok(ApiResponse<string>.Success(value));
    }

    [HttpPost]
    public async Task<IActionResult> CreateSetting([FromBody] CreateSystemSettingDto dto)
    {
        // Check if key already exists
        var existingSetting = await _context.SystemSettings
            .FirstOrDefaultAsync(s => s.Key == dto.Key);

        if (existingSetting != null)
            return BadRequest(ApiResponse<SystemSetting>.Error("Setting with this key already exists"));

        var setting = new SystemSetting
        {
            Key = dto.Key,
            Value = dto.Value,
            DefaultValue = dto.DefaultValue,
            DataType = dto.DataType ?? "string",
            Category = dto.Category ?? "general",
            Description = dto.Description,
            IsEditable = dto.IsEditable,
            IsVisible = dto.IsVisible,
            IsRequired = dto.IsRequired,
            ValidationRules = dto.ValidationRules,
            Options = dto.Options,
            SortOrder = dto.SortOrder,
            Group = dto.Group,
            Section = dto.Section
        };

        _context.SystemSettings.Add(setting);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSettingByKey), new { key = setting.Key }, ApiResponse<SystemSetting>.Success(setting));
    }

    [HttpPut("key/{key}")]
    public async Task<IActionResult> UpdateSetting(string key, [FromBody] UpdateSystemSettingDto dto)
    {
        var setting = await _context.SystemSettings
            .FirstOrDefaultAsync(s => s.Key == key);

        if (setting == null)
            return NotFound(ApiResponse<SystemSetting>.Error("Setting not found"));

        if (!setting.IsEditable)
            return BadRequest(ApiResponse<SystemSetting>.Error("This setting is not editable"));

        if (dto.Value != null) setting.Value = dto.Value;
        if (dto.Description != null) setting.Description = dto.Description;
        if (dto.IsVisible.HasValue) setting.IsVisible = dto.IsVisible.Value;
        if (dto.ValidationRules != null) setting.ValidationRules = dto.ValidationRules;
        if (dto.Options != null) setting.Options = dto.Options;
        if (dto.SortOrder.HasValue) setting.SortOrder = dto.SortOrder.Value;
        if (dto.Group != null) setting.Group = dto.Group;
        if (dto.Section != null) setting.Section = dto.Section;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<SystemSetting>.Success(setting));
    }

    [HttpPut("bulk")]
    public async Task<IActionResult> UpdateMultipleSettings([FromBody] Dictionary<string, string> settings)
    {
        var updatedSettings = new List<SystemSetting>();

        foreach (var kvp in settings)
        {
            var setting = await _context.SystemSettings
                .FirstOrDefaultAsync(s => s.Key == kvp.Key);

            if (setting != null && setting.IsEditable)
            {
                setting.Value = kvp.Value;
                updatedSettings.Add(setting);
            }
        }

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<List<SystemSetting>>.Success(updatedSettings));
    }

    [HttpDelete("key/{key}")]
    public async Task<IActionResult> DeleteSetting(string key)
    {
        var setting = await _context.SystemSettings
            .FirstOrDefaultAsync(s => s.Key == key);

        if (setting == null)
            return NotFound(ApiResponse<SystemSetting>.Error("Setting not found"));

        if (setting.IsRequired)
            return BadRequest(ApiResponse<SystemSetting>.Error("Cannot delete required setting"));

        _context.SystemSettings.Remove(setting);
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<string>.Success("Setting deleted successfully"));
    }

    [HttpPost("reset/{key}")]
    public async Task<IActionResult> ResetSetting(string key)
    {
        var setting = await _context.SystemSettings
            .FirstOrDefaultAsync(s => s.Key == key);

        if (setting == null)
            return NotFound(ApiResponse<SystemSetting>.Error("Setting not found"));

        setting.Value = setting.DefaultValue;
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<SystemSetting>.Success(setting));
    }
}

// DTOs
public class CreateSystemSettingDto
{
    public string Key { get; set; } = string.Empty;
    public string? Value { get; set; }
    public string? DefaultValue { get; set; }
    public string? DataType { get; set; } = "string";
    public string? Category { get; set; } = "general";
    public string? Description { get; set; }
    public bool IsEditable { get; set; } = true;
    public bool IsVisible { get; set; } = true;
    public bool IsRequired { get; set; } = false;
    public string? ValidationRules { get; set; }
    public string? Options { get; set; }
    public int SortOrder { get; set; } = 0;
    public string? Group { get; set; }
    public string? Section { get; set; }
}

public class UpdateSystemSettingDto
{
    public string? Value { get; set; }
    public string? Description { get; set; }
    public bool? IsVisible { get; set; }
    public string? ValidationRules { get; set; }
    public string? Options { get; set; }
    public int? SortOrder { get; set; }
    public string? Group { get; set; }
    public string? Section { get; set; }
}