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

    public async Task<ApiResponse<IEnumerable<CompanyDto>>> GetAllCompaniesAsync(int? tenantId = null)
    {
        try
        {
            var query = _context.Companies.Where(x => !x.IsDeleted).AsQueryable();
            if (tenantId.HasValue)
                query = query.Where(c => c.TenantId == tenantId.Value);
                
            var companies = await query.ToListAsync();
            var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return new ApiResponse<IEnumerable<CompanyDto>> { Data = companyDtos, IsSuccess = true, Message = "Companies retrieved successfully" };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<CompanyDto>> { Data = null, IsSuccess = false, Message = $"Error retrieving companies: {ex.Message}" };
        }
    }

    public async Task<ApiResponse<CompanyDto>> GetCompanyByIdAsync(Guid id)
    {
        try
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
                return new ApiResponse<CompanyDto> { Data = null, IsSuccess = false, Message = "Company not found" };

            var companyDto = _mapper.Map<CompanyDto>(company);
            return new ApiResponse<CompanyDto> { Data = companyDto, IsSuccess = true, Message = "Company retrieved successfully" };
        }
        catch (Exception ex)
        {
            return new ApiResponse<CompanyDto> { Data = null, IsSuccess = false, Message = $"Error retrieving company: {ex.Message}" };
        }
    }

    public async Task<ApiResponse<CompanyDto>> GetCompanyByCodeAsync(string code)
    {
        try
        {
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Code == code);
            if (company == null)
                return new ApiResponse<CompanyDto> { Data = null, IsSuccess = false, Message = "Company not found" };

            var companyDto = _mapper.Map<CompanyDto>(company);
            return new ApiResponse<CompanyDto> { Data = companyDto, IsSuccess = true, Message = "Company retrieved successfully" };
        }
        catch (Exception ex)
        {
            return new ApiResponse<CompanyDto> { Data = null, IsSuccess = false, Message = $"Error retrieving company: {ex.Message}" };
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
            return new ApiResponse<CompanyDto> { Data = companyDto, IsSuccess = true, Message = "Company created successfully" };
        }
        catch (Exception ex)
        {
            return new ApiResponse<CompanyDto> { Data = null, IsSuccess = false, Message = $"Error creating company: {ex.Message}" };
        }
    }

    public async Task<ApiResponse<CompanyDto>> UpdateCompanyAsync(Guid id, UpdateCompanyDto updateCompanyDto)
    {
        try
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
                return new ApiResponse<CompanyDto> { Data = null, IsSuccess = false, Message = "Company not found" };

            _mapper.Map(updateCompanyDto, company);
            company.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var companyDto = _mapper.Map<CompanyDto>(company);
            return new ApiResponse<CompanyDto> { Data = companyDto, IsSuccess = true, Message = "Company updated successfully" };
        }
        catch (Exception ex)
        {
            return new ApiResponse<CompanyDto> { Data = null, IsSuccess = false, Message = $"Error updating company: {ex.Message}" };
        }
    }

    public async Task<ApiResponse<bool>> DeleteCompanyAsync(Guid id)
    {
        try
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
                return new ApiResponse<bool> { Data = false, IsSuccess = false, Message = "Company not found" };

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool> { Data = true, IsSuccess = true, Message = "Company deleted successfully" };
        }
        catch (Exception ex)
        {
            return new ApiResponse<bool> { Data = false, IsSuccess = false, Message = $"Error deleting company: {ex.Message}" };
        }
    }
}
