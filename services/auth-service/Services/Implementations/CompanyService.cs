using AuthService.Data;
using AuthService.DTOs.Company;
using SharedLibrary.DTOs;
using AuthService.Interfaces;
using AuthService.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace AuthService.Services.Implementations
{
    public class CompanyService : ICompanyService
    {
        private readonly AuthDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateCompanyRequest> _createValidator;
        private readonly IValidator<UpdateCompanyRequest> _updateValidator;

        public CompanyService(
            AuthDbContext context,
            IMapper mapper,
            IValidator<CreateCompanyRequest> createValidator,
            IValidator<UpdateCompanyRequest> updateValidator)
        {
            _context = context;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<CompanyResponse> CreateAsync(CreateCompanyRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

            var company = _mapper.Map<Company>(request);
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
            return _mapper.Map<CompanyResponse>(company);
        }

        public async Task<CompanyResponse> UpdateAsync(int id, UpdateCompanyRequest request)
        {
            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

            var company = await _context.Companies.FindAsync(id)
                ?? throw new KeyNotFoundException("Company not found");

            _mapper.Map(request, company);
            company.ChangedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return _mapper.Map<CompanyResponse>(company);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var company = await _context.Companies.FindAsync(id)
                ?? throw new KeyNotFoundException("Company not found");

            company.IsDeleted = true;
            company.ChangedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CompanyResponse> GetByIdAsync(int id)
        {
            var company = await _context.Companies.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted)
                ?? throw new KeyNotFoundException("Company not found");

            return _mapper.Map<CompanyResponse>(company);
        }

        public async Task<PaginatedResponse<CompanyResponse>> GetAllAsync(CompanyFilterRequest filter)
        {
            var query = _context.Companies.AsQueryable().Where(c => !c.IsDeleted);

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(c => c.Name.Contains(filter.Name));

            var totalCount = await query.CountAsync();
            var data = await query
                .OrderBy(c => c.Name)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var result = _mapper.Map<List<CompanyResponse>>(data);
            return new PaginatedResponse<CompanyResponse>(result, totalCount, filter.Page, filter.PageSize);
        }
    }
}