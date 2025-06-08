namespace AuthService.DTOs.UserPermission
{
    public class UpdateUserPermissionRequest
    {
        public int UserId { get; set; }
        public int PermissionId { get; set; }
    }

}