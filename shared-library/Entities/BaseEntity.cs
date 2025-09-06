using System;

namespace SharedLibrary.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        // Audit trail
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public bool IsActive { get; set; } = true;

        // Soft delete flag
        public bool IsDeleted { get; set; } = false;

        // Optional concurrency token (untuk optimistic concurrency)
        public byte[]? RowVersion { get; set; }

        public int? TenantId { get; set; }
    }
}
