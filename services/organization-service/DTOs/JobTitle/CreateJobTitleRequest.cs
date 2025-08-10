namespace OrganizationService.DTOs.JobTitle
{
    public class CreateJobTitleRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid CompanyId { get; set; }
    }
}