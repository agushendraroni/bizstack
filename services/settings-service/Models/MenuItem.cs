using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace SettingsService.Models;

public class MenuItem : BaseEntity
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
    public string? Roles { get; set; } // JSON array of roles
    public string? Target { get; set; } = "_self"; // _self, _blank, etc
    public string? CssClass { get; set; }
    public string? Badge { get; set; }
    public string? BadgeColor { get; set; }
    
    // Hierarchical structure
    public Guid? ParentId { get; set; }
    public MenuItem? Parent { get; set; }
    public ICollection<MenuItem> Children { get; set; } = new List<MenuItem>();
    
    // Menu context (where this menu belongs)
    public string MenuContext { get; set; } = "main"; // main, sidebar, footer, etc
    
    // Additional metadata
    public string? Metadata { get; set; } // JSON for additional properties
}