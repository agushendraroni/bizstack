namespace UserService.DTOs.UserPreference;
using System;


public class UserPreferenceResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Language { get; set; } = default!;
    public string Theme { get; set; } = default!;
    public bool ReceiveNotifications { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ChangedAt { get; set; }
    public string? ChangedBy { get; set; }
}