namespace OrganizationService.DTOs.BusinessGroup
{
    public class CreateBusinessGroupRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}