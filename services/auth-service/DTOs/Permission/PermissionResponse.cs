namespace AuthService.DTOs.Permission;


 public class PermissionResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = string.Empty;
        public int CompanyId { get; set; }
    }