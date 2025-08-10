using SharedLibrary.DTOs;

namespace OrganizationService.DTOs.Branch
{
    public class BranchFilterRequest : PaginationFilter
    {
        public string? Name { get; set; }
        public Guid? CompanyId { get; set; }
    }
}