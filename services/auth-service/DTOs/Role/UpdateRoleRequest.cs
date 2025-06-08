namespace AuthService.DTOs.Role
{
    public class UpdateRoleRequest
    {
        public string Name { get; set; } = string.Empty;
        public int CompanyId { get; set; }

         public bool IsActive { get; set; }

    }
}
