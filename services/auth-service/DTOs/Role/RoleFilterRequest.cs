using SharedLibrary.DTOs;

namespace AuthService.DTOs.Role
{
    public class RoleFilterRequest : PaginationFilter
    {
        public string? Name { get; set; }
        public int? CompanyId { get; set; }
    }
}
