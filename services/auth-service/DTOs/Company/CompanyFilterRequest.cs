using AuthService.DTOs.Common;

namespace AuthService.DTOs.Company
{
    public class CompanyFilterRequest : PaginationFilter
    {
        public string? Name { get; set; }
    }
}