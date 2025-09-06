using Microsoft.EntityFrameworkCore;
using AutoMapper;
using OrganizationService.Data;
using OrganizationService.DTOs;
using OrganizationService.Models;
using SharedLibrary.DTOs;

namespace OrganizationService.Services;

public class CompanyService : ICompanyService
{
    private readonly OrganizationDbContext _context;
    private readonly IMapper _mapper;

    public CompanyService(OrganizationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<CompanyDto>>> GetAllCompaniesAsync()
    {
        try
        {
            var companies = await _context.Companies.ToListAsync();
            var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return ApiResponse<IEnumerable<CompanyDto>>.Success(companyDtos);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<CompanyDto>>.Error($"Error retrieving companies: {ex.Message}");
        }
    }

    public async Task<ApiResponse<CompanyDto>> GetCompanyByIdAsync(Guid id)
    {
        try
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
                return ApiResponse<CompanyDto>.Error("Company not found");

            var companyDto = _mapper.Map<CompanyDto>(company);
            return ApiResponse<CompanyDto>.Success(companyDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<CompanyDto>.Error($"Error retrieving company: {ex.Message}");
        }
    }

    public async Task<ApiResponse<CompanyDto>> GetCompanyByCodeAsync(string code)
    {
        try
        {
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Code == code);
            if (company == null)
                return ApiResponse<CompanyDto>.Error("Company not found");

            var companyDto = _mapper.Map<CompanyDto>(company);
            return ApiResponse<CompanyDto>.Success(companyDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<CompanyDto>.Error($"Error retrieving company: {ex.Message}");
        }
    }

    public async Task<ApiResponse<CompanyDto>> CreateCompanyAsync(CreateCompanyDto createCompanyDto)
    {
        try
        {
            var company = _mapper.Map<Company>(createCompanyDto);
            company.Id = Guid.NewGuid();
            company.CreatedAt = DateTime.UtcNow;
            company.IsActive = true;

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            var companyDto = _mapper.Map<CompanyDto>(company);
            return ApiResponse<CompanyDto>.Success(companyDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<CompanyDto>.Error($"Error creating company: {ex.Message}");
        }
    }

    public async Task<ApiResponse<CompanyDto>> UpdateCompanyAsync(Guid id, UpdateCompanyDto updateCompanyDto)
    {
        try
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
                return ApiResponse<CompanyDto>.Error("Company not found");

            _mapper.Map(updateCompanyDto, company);
            company.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var companyDto = _mapper.Map<CompanyDto>(company);
            return ApiResponse<CompanyDto>.Success(companyDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<CompanyDto>.Error($"Error updating company: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DeleteCompanyAsync(Guid id)
    {
        try
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
                return ApiResponse<bool>.Error("Company not found");

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.Error($"Error deleting company: {ex.Message}");
        }
    }
}
