using OrganizationService.Data;
using OrganizationService.DTOs.Branch;
using OrganizationService.Models;
using OrganizationService.Services.Interfaces;
using SharedLibrary.DTOs;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace OrganizationService.Services.Implementations
{
    public class BranchService(
        OrganizationDbContext _context,
        IMapper _mapper,
        IValidator<CreateBranchRequest> _createValidator,
        IValidator<UpdateBranchRequest> _updateValidator
    ) : IBranchService
    {
        public async Task<BranchResponse> CreateAsync(CreateBranchRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

            var branch = _mapper.Map<Branch>(request);
            _context.Branches.Add(branch);
            await _context.SaveChangesAsync();
            return _mapper.Map<BranchResponse>(branch);
        }

        public async Task<BranchResponse> UpdateAsync(Guid id, UpdateBranchRequest request)
        {
            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

            var branch = await _context.Branches.FindAsync(id)
                ?? throw new KeyNotFoundException("Branch not found");

            _mapper.Map(request, branch);
            branch.ChangedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return _mapper.Map<BranchResponse>(branch);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var branch = await _context.Branches.FindAsync(id)
                ?? throw new KeyNotFoundException("Branch not found");

            branch.IsDeleted = true;
            branch.ChangedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<BranchResponse> GetByIdAsync(Guid id)
        {
            var branch = await _context.Branches.AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted)
                ?? throw new KeyNotFoundException("Branch not found");

            return _mapper.Map<BranchResponse>(branch);
        }

        public async Task<PaginatedResponse<BranchResponse>> GetAllAsync(BranchFilterRequest filter)
        {
            var query = _context.Branches.AsQueryable().Where(b => !b.IsDeleted);

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(b => b.Name.Contains(filter.Name));
            if (filter.CompanyId.HasValue)
                query = query.Where(b => b.CompanyId == filter.CompanyId);

            var totalCount = await query.CountAsync();
            var data = await query
                .OrderBy(b => b.Name)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var result = _mapper.Map<List<BranchResponse>>(data);
            return new PaginatedResponse<BranchResponse>(result, totalCount, filter.Page, filter.PageSize);
        }
    }
}