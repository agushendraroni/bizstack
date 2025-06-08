using System;

namespace AuthService.DTOs.UserRole
{
    public class UserRoleResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ChangedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? ChangedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}