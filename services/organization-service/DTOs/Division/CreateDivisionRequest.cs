namespace OrganizationService.DTOs.Division
{
    public class CreateDivisionRequest
    {
        public string Name { get; set; } = string.Empty;
        public Guid CompanyId { get; set; }
    }
}