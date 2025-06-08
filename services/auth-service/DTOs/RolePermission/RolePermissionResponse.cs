namespace AuthService.DTOs.RolePermission;
 public class RolePermissionResponse
{
    public int RoleId { get; set; }
    public int PermissionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ChangedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? ChangedBy { get; set; }
}