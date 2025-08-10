namespace OrganizationService.DTOs.CostCenter
{
    public class CreateCostCenterRequest
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public Guid DivisionId { get; set; }
    }
}