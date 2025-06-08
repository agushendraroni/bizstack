using SharedLibrary.DTOs;
using AuthService.Models;

namespace AuthService.DTOs.Permission
{
    public class PermissionFilterRequest : PaginationFilter
    {
        public string? Search { get; set; }
        public int? CompanyId { get; set; }
        public PermissionType? Type { get; set; }
   }
}
