using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace SettingsService.Models;

public class Configuration : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public string ConfigData { get; set; } = "{}"; // JSON configuration
    public string ConfigType { get; set; } = "json";
    public string Category { get; set; } = "general";
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsReadOnly { get; set; } = false;
    public string? Version { get; set; }
    public string? Environment { get; set; } = "production"; // development, staging, production
    public DateTime? EffectiveDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
}