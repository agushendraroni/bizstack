using SharedLibrary.DTOs;

namespace OrganizationService.DTOs.LegalDocument
{
    public class LegalDocumentFilterRequest : PaginationFilter
    {
        public Guid? CompanyId { get; set; }
        public string? DocumentType { get; set; }
        public string? DocumentNumber { get; set; }
    }
}