namespace AuthService.DTOs.Role;

public class RoleFilterRequest
{
    public int? CompanyId { get; set; }
    public bool? IsActive { get; set; }
    public string? NameContains { get; set; }
    public int? Page { get; set; } = 1;
    public int? PageSize { get; set; } = 10;
}