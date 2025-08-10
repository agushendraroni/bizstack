using SharedLibrary.DTOs;

namespace OrganizationService.DTOs.JobTitle
{
    public class JobTitleFilterRequest : PaginationFilter
    {
        public string? Title { get; set; }
        public Guid? CompanyId { get; set; }
    }
}