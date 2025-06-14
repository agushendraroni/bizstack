using SharedLibrary.DTOs;

namespace OrganizationService.DTOs.Company{
    public class CompanyFilterRequest : PaginationFilter
    {
        public string? Name { get; set; }
    }
}