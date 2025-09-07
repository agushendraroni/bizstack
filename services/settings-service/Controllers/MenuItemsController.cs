using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using SettingsService.Data;
using SettingsService.Models;
using SharedLibrary.DTOs;

namespace SettingsService.Controllers;

[ApiController]
[ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
public class MenuItemsController : ControllerBase
{
    private readonly SettingsDbContext _context;

    public MenuItemsController(SettingsDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetMenuItems([FromQuery] string? context = "main")
    {
        var menuItems = await _context.MenuItems
            .Where(m => m.MenuContext == context && m.IsActive)
            .OrderBy(m => m.SortOrder)
            .ToListAsync();

        var menuTree = BuildMenuTree(menuItems);
        return Ok(ApiResponse<List<MenuItem>>.Success(menuTree));
    }

    [HttpGet("flat")]
    public async Task<IActionResult> GetMenuItemsFlat([FromQuery] string? context = "main")
    {
        var menuItems = await _context.MenuItems
            .Where(m => m.MenuContext == context)
            .OrderBy(m => m.Level).ThenBy(m => m.SortOrder)
            .ToListAsync();

        return Ok(ApiResponse<List<MenuItem>>.Success(menuItems));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMenuItem(Guid id)
    {
        var menuItem = await _context.MenuItems
            .Include(m => m.Parent)
            .Include(m => m.Children)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (menuItem == null)
            return NotFound(ApiResponse<MenuItem>.Error("Menu item not found"));

        return Ok(ApiResponse<MenuItem>.Success(menuItem));
    }

    [HttpGet("tree/{context}")]
    public async Task<IActionResult> GetMenuTree(string context = "main")
    {
        var menuItems = await _context.MenuItems
            .Where(m => m.MenuContext == context && m.IsActive && m.IsVisible)
            .OrderBy(m => m.SortOrder)
            .ToListAsync();

        var menuTree = BuildMenuTree(menuItems);
        return Ok(ApiResponse<List<MenuItem>>.Success(menuTree));
    }

    [HttpPost]
    public async Task<IActionResult> CreateMenuItem([FromBody] CreateMenuItemDto dto)
    {
        var menuItem = new MenuItem
        {
            Name = dto.Name,
            DisplayName = dto.DisplayName,
            Description = dto.Description,
            Icon = dto.Icon,
            Url = dto.Url,
            Route = dto.Route,
            Component = dto.Component,
            SortOrder = dto.SortOrder,
            Level = dto.Level,
            IsActive = dto.IsActive,
            IsVisible = dto.IsVisible,
            Permission = dto.Permission,
            Roles = dto.Roles,
            Target = dto.Target,
            CssClass = dto.CssClass,
            Badge = dto.Badge,
            BadgeColor = dto.BadgeColor,
            ParentId = dto.ParentId,
            MenuContext = dto.MenuContext ?? "main",
            Metadata = dto.Metadata
        };

        // Auto-calculate level if parent is specified
        if (dto.ParentId.HasValue)
        {
            var parent = await _context.MenuItems.FindAsync(dto.ParentId.Value);
            if (parent != null)
            {
                menuItem.Level = parent.Level + 1;
            }
        }

        _context.MenuItems.Add(menuItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMenuItem), new { id = menuItem.Id }, ApiResponse<MenuItem>.Success(menuItem));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMenuItem(Guid id, [FromBody] UpdateMenuItemDto dto)
    {
        var menuItem = await _context.MenuItems.FindAsync(id);
        if (menuItem == null)
            return NotFound(ApiResponse<MenuItem>.Error("Menu item not found"));

        if (!string.IsNullOrEmpty(dto.Name)) menuItem.Name = dto.Name;
        if (dto.DisplayName != null) menuItem.DisplayName = dto.DisplayName;
        if (dto.Description != null) menuItem.Description = dto.Description;
        if (dto.Icon != null) menuItem.Icon = dto.Icon;
        if (dto.Url != null) menuItem.Url = dto.Url;
        if (dto.Route != null) menuItem.Route = dto.Route;
        if (dto.Component != null) menuItem.Component = dto.Component;
        if (dto.SortOrder.HasValue) menuItem.SortOrder = dto.SortOrder.Value;
        if (dto.IsActive.HasValue) menuItem.IsActive = dto.IsActive.Value;
        if (dto.IsVisible.HasValue) menuItem.IsVisible = dto.IsVisible.Value;
        if (dto.Permission != null) menuItem.Permission = dto.Permission;
        if (dto.Roles != null) menuItem.Roles = dto.Roles;
        if (dto.Target != null) menuItem.Target = dto.Target;
        if (dto.CssClass != null) menuItem.CssClass = dto.CssClass;
        if (dto.Badge != null) menuItem.Badge = dto.Badge;
        if (dto.BadgeColor != null) menuItem.BadgeColor = dto.BadgeColor;
        if (dto.Metadata != null) menuItem.Metadata = dto.Metadata;

        // Handle parent change
        if (dto.ParentId != menuItem.ParentId)
        {
            menuItem.ParentId = dto.ParentId;
            if (dto.ParentId.HasValue)
            {
                var parent = await _context.MenuItems.FindAsync(dto.ParentId.Value);
                if (parent != null)
                {
                    menuItem.Level = parent.Level + 1;
                }
            }
            else
            {
                menuItem.Level = 1;
            }
        }

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<MenuItem>.Success(menuItem));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMenuItem(Guid id)
    {
        var menuItem = await _context.MenuItems
            .Include(m => m.Children)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (menuItem == null)
            return NotFound(ApiResponse<MenuItem>.Error("Menu item not found"));

        // Check if has children
        if (menuItem.Children.Any())
        {
            return BadRequest(ApiResponse<MenuItem>.Error("Cannot delete menu item with children. Delete children first."));
        }

        _context.MenuItems.Remove(menuItem);
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<string>.Success("Menu item deleted successfully"));
    }

    [HttpPost("{id}/move")]
    public async Task<IActionResult> MoveMenuItem(Guid id, [FromBody] MoveMenuItemDto dto)
    {
        var menuItem = await _context.MenuItems.FindAsync(id);
        if (menuItem == null)
            return NotFound(ApiResponse<MenuItem>.Error("Menu item not found"));

        menuItem.ParentId = dto.NewParentId;
        menuItem.SortOrder = dto.NewSortOrder;

        // Recalculate level
        if (dto.NewParentId.HasValue)
        {
            var parent = await _context.MenuItems.FindAsync(dto.NewParentId.Value);
            if (parent != null)
            {
                menuItem.Level = parent.Level + 1;
            }
        }
        else
        {
            menuItem.Level = 1;
        }

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<MenuItem>.Success(menuItem));
    }

    private List<MenuItem> BuildMenuTree(List<MenuItem> menuItems)
    {
        var menuDict = menuItems.ToDictionary(m => m.Id, m => m);
        var rootItems = new List<MenuItem>();

        foreach (var item in menuItems)
        {
            if (item.ParentId == null)
            {
                rootItems.Add(item);
            }
            else if (menuDict.ContainsKey(item.ParentId.Value))
            {
                var parent = menuDict[item.ParentId.Value];
                parent.Children.Add(item);
            }
        }

        return rootItems.OrderBy(m => m.SortOrder).ToList();
    }
}

// DTOs
public class CreateMenuItemDto
{
    public string Name { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string? Url { get; set; }
    public string? Route { get; set; }
    public string? Component { get; set; }
    public int SortOrder { get; set; } = 0;
    public int Level { get; set; } = 1;
    public bool IsActive { get; set; } = true;
    public bool IsVisible { get; set; } = true;
    public string? Permission { get; set; }
    public string? Roles { get; set; }
    public string? Target { get; set; } = "_self";
    public string? CssClass { get; set; }
    public string? Badge { get; set; }
    public string? BadgeColor { get; set; }
    public Guid? ParentId { get; set; }
    public string? MenuContext { get; set; } = "main";
    public string? Metadata { get; set; }
}

public class UpdateMenuItemDto
{
    public string? Name { get; set; }
    public string? DisplayName { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string? Url { get; set; }
    public string? Route { get; set; }
    public string? Component { get; set; }
    public int? SortOrder { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsVisible { get; set; }
    public string? Permission { get; set; }
    public string? Roles { get; set; }
    public string? Target { get; set; }
    public string? CssClass { get; set; }
    public string? Badge { get; set; }
    public string? BadgeColor { get; set; }
    public Guid? ParentId { get; set; }
    public string? Metadata { get; set; }
}

public class MoveMenuItemDto
{
    public Guid? NewParentId { get; set; }
    public int NewSortOrder { get; set; }

    private int? GetTenantId()
    {
        return null;
    }

    private Guid? GetUserId()
    {
        return null;
    }
}