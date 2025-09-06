using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace SettingsService.Models;

public class Theme : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public string? Description { get; set; }
    public string PrimaryColor { get; set; } = "#007bff";
    public string SecondaryColor { get; set; } = "#6c757d";
    public string BackgroundColor { get; set; } = "#ffffff";
    public string TextColor { get; set; } = "#212529";
    public string? Logo { get; set; }
    public string? Favicon { get; set; }
    public string? CustomCss { get; set; }
    public string? CustomJs { get; set; }
    public bool IsDefault { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public string? PreviewImage { get; set; }
    public string? FontFamily { get; set; }
    public string? AdditionalSettings { get; set; } // JSON for extra theme settings
}