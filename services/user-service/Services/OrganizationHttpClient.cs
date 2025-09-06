using System.Text.Json;

namespace UserService.Services;

public class OrganizationHttpClient : IOrganizationHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OrganizationHttpClient> _logger;

    public OrganizationHttpClient(HttpClient httpClient, ILogger<OrganizationHttpClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<OrganizationDto?> GetCompanyByIdAsync(Guid companyId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/companies/{companyId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponseWrapper<OrganizationDto>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return apiResponse?.Data;
            }
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting company {CompanyId}", companyId);
            return null;
        }
    }

    public async Task<bool> CompanyExistsAsync(Guid companyId)
    {
        var company = await GetCompanyByIdAsync(companyId);
        return company != null && company.IsActive;
    }
}

public class ApiResponseWrapper<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
}
