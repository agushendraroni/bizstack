using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace FileStorageService.Models;

public class FileRecord : BaseEntity
{
    [Required, MaxLength(255)]
    public string FileName { get; set; } = string.Empty;

    [Required, MaxLength(255)]
    public string OriginalFileName { get; set; } = string.Empty;

    [Required, MaxLength(500)]
    public string FilePath { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string ContentType { get; set; } = string.Empty;

    public long FileSize { get; set; }

    [MaxLength(50)]
    public string Category { get; set; } = "General"; // Product, Receipt, Logo, Document

    [MaxLength(500)]
    public string? Description { get; set; }

    public Guid? RelatedEntityId { get; set; }

    [MaxLength(50)]
    public string? RelatedEntityType { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public Guid UploadedBy { get; set; }
}
