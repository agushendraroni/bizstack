namespace UserService.Services;

public interface IOrganizationHttpClient
{
    Task<OrganizationDto?> GetCompanyByIdAsync(Guid companyId);
    Task<bool> CompanyExistsAsync(Guid companyId);
}

public class OrganizationDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
