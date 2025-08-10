using SharedLibrary.DTOs;

namespace OrganizationService.DTOs.Division
{
    public class DivisionFilterRequest : PaginationFilter
    {
        public string? Name { get; set; }
        public Guid? CompanyId { get; set; }
    }
}