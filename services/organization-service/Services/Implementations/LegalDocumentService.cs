using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OrganizationService.Data;
using OrganizationService.DTOs.LegalDocument;
using OrganizationService.Models;
using OrganizationService.Services.Interfaces;
using SharedLibrary.DTOs;

namespace OrganizationService.Services.Implementations
{
    public class LegalDocumentService : ILegalDocumentService
    {
        private readonly OrganizationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateLegalDocumentRequest> _createValidator;
        private readonly IValidator<UpdateLegalDocumentRequest> _updateValidator;

        public LegalDocumentService(
            OrganizationDbContext context,
            IMapper mapper,
            IValidator<CreateLegalDocumentRequest> createValidator,
            IValidator<UpdateLegalDocumentRequest> updateValidator)
        {
            _context = context;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<LegalDocumentResponse> CreateAsync(CreateLegalDocumentRequest request)
        {
            var validation = await _createValidator.ValidateAsync(request);
            if (!validation.IsValid)
                throw new ValidationException(string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));

            var entity = _mapper.Map<LegalDocument>(request);
            _context.LegalDocuments.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<LegalDocumentResponse>(entity);
        }

        public async Task<LegalDocumentResponse> UpdateAsync(Guid id, UpdateLegalDocumentRequest request)
        {
            var validation = await _updateValidator.ValidateAsync(request);
            if (!validation.IsValid)
                throw new ValidationException(string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));

            var entity = await _context.LegalDocuments.FindAsync(id)
                ?? throw new KeyNotFoundException("LegalDocument not found");

            _mapper.Map(request, entity);
            entity.ChangedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return _mapper.Map<LegalDocumentResponse>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.LegalDocuments.FindAsync(id)
                ?? throw new KeyNotFoundException("LegalDocument not found");

            entity.IsDeleted = true;
            entity.ChangedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<LegalDocumentResponse> GetByIdAsync(Guid id)
        {
            var entity = await _context.LegalDocuments.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
                ?? throw new KeyNotFoundException("LegalDocument not found");

            return _mapper.Map<LegalDocumentResponse>(entity);
        }

        public async Task<PaginatedResponse<LegalDocumentResponse>> GetAllAsync(LegalDocumentFilterRequest filter)
        {
            var query = _context.LegalDocuments.AsQueryable().Where(x => !x.IsDeleted);

            if (filter.CompanyId.HasValue)
                query = query.Where(x => x.CompanyId == filter.CompanyId);
            if (!string.IsNullOrEmpty(filter.DocumentType))
                query = query.Where(x => x.DocumentType.Contains(filter.DocumentType));
            if (!string.IsNullOrEmpty(filter.DocumentNumber))
                query = query.Where(x => x.DocumentNumber.Contains(filter.DocumentNumber));

            var totalCount = await query.CountAsync();
            var data = await query
                .OrderBy(x => x.DocumentType)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var result = _mapper.Map<List<LegalDocumentResponse>>(data);
            return new PaginatedResponse<LegalDocumentResponse>(result, totalCount, filter.Page, filter.PageSize);
        }
    }
}