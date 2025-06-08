using System;

namespace SharedLibrary.Entities
{
    public abstract class BaseEntity
    {
 

        // Audit trail
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ChangedAt { get; set; }

        public string? CreatedBy { get; set; }
        public string? ChangedBy { get; set; }

        public bool IsActive { get; set; } = false;

        // Soft delete flag
        public bool IsDeleted { get; set; } = false;

        // Optional concurrency token (untuk optimistic concurrency)
        public byte[]? RowVersion { get; set; }

        public int? TenantId { get; set; }

    }
}
