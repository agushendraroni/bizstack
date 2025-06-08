using SharedLibrary.DTOs;

namespace AuthService.DTOs.Company
{
    public class CompanyFilterRequest : PaginationFilter
    {
        public string? Name { get; set; }
    }
}