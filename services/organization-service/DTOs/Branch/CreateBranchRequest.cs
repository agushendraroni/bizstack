namespace OrganizationService.DTOs.Branch
{
    public class CreateBranchRequest
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public Guid CompanyId { get; set; }
    }
}