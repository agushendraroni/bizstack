namespace AuthService.DTOs.UserPermission
{
   public class UserPermissionResponse
    {
        public int UserId { get; set; }
        public int PermissionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ChangedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? ChangedBy { get; set; }
        public bool IsDeleted { get; set; }
    }

}
