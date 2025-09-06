using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace SettingsService.Models;

public class SystemSetting : BaseEntity
{
    public string Key { get; set; } = string.Empty;
    public string? Value { get; set; }
    public string? DefaultValue { get; set; }
    public string DataType { get; set; } = "string"; // string, int, bool, json, etc
    public string Category { get; set; } = "general";
    public string? Description { get; set; }
    public bool IsEditable { get; set; } = true;
    public bool IsVisible { get; set; } = true;
    public bool IsRequired { get; set; } = false;
    public string? ValidationRules { get; set; } // JSON validation rules
    public string? Options { get; set; } // JSON for dropdown options
    public int SortOrder { get; set; } = 0;
    public string? Group { get; set; }
    public string? Section { get; set; }
}