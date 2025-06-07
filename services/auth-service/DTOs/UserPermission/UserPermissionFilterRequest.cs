namespace AuthService.DTOs.UserPermission
{
    public class UserPermissionFilterRequest
    {
        public int? UserId { get; set; }
        public int? PermissionId { get; set; }
    }
}