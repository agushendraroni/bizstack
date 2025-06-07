namespace AuthService.DTOs.UserPermission
{
    public class CreateUserPermissionRequest
    {
        public int UserId { get; set; }
        public int PermissionId { get; set; }
    }
}