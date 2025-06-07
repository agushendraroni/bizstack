using System;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.Base
{
   public abstract class BaseEntity
{
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ChangedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? ChangedBy { get; set; }
}

}
