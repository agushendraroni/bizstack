using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace NotificationService.Models;

public class Notification : BaseEntity
{
    [Required, MaxLength(50)]
    public string Type { get; set; } = string.Empty; // Email, SMS, WhatsApp, Push

    [Required, MaxLength(200)]
    public string Recipient { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    public string Message { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Status { get; set; } = "Pending"; // Pending, Sent, Failed, Delivered

    public DateTime? SentAt { get; set; }

    [MaxLength(500)]
    public string? ErrorMessage { get; set; }

    public int RetryCount { get; set; } = 0;

    public Guid? RelatedEntityId { get; set; } // Order ID, Customer ID, etc.

    [MaxLength(50)]
    public string? RelatedEntityType { get; set; } // Order, Customer, etc.
}
