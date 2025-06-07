using AuthService.DTOs.Company;

namespace AuthService.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyResponse>> GetAllAsync(CompanyFilterRequest filter);
        Task<CompanyResponse?> GetByIdAsync(int id);
        Task<CompanyResponse> CreateAsync(CreateCompanyRequest request);
        Task<CompanyResponse?> UpdateAsync(int id, UpdateCompanyRequest request);
        Task<bool> DeleteAsync(int id);
    }
}