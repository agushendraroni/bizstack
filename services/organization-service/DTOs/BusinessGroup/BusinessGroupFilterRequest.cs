using SharedLibrary.DTOs;

namespace OrganizationService.DTOs.BusinessGroup
{
    public class BusinessGroupFilterRequest : PaginationFilter
    {
        public string? Name { get; set; }
    }
}