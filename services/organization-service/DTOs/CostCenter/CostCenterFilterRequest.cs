using SharedLibrary.DTOs;

namespace OrganizationService.DTOs.CostCenter
{
    public class CostCenterFilterRequest : PaginationFilter
    {
        public string? Name { get; set; }
        public Guid? DivisionId { get; set; }
    }
}