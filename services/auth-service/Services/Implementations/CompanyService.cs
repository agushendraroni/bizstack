using AuthService.Data;
using AuthService.DTOs.Company;
using AuthService.Models;
using AuthService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services.Implementations
{
    public class CompanyService : ICompanyService
    {
        private readonly AuthDbContext _context;

        public CompanyService(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<CompanyResponse> CreateAsync(CreateCompanyRequest request)
        {
            var company = new Company
            {
                Name = request.Name,
                IsActive = true
            };

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return MapToResponse(company);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null) return false;

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CompanyResponse>> GetAllAsync(CompanyFilterRequest filter)
        {
            var query = _context.Companies.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(c => c.Name.Contains(filter.Name));

            return (await query.ToListAsync()).Select(MapToResponse);
        }

        public async Task<CompanyResponse?> GetByIdAsync(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            return company == null ? null : MapToResponse(company);
        }

        public async Task<CompanyResponse?> UpdateAsync(int id, UpdateCompanyRequest request)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null) return null;

            company.Name = request.Name;
            await _context.SaveChangesAsync();

            return MapToResponse(company);
        }

        private CompanyResponse MapToResponse(Company entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            IsActive = entity.IsActive
        };
    }
}