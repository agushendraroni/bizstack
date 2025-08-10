namespace OrganizationService.DTOs.LegalDocument
{
    public class CreateLegalDocumentRequest
    {
        public Guid CompanyId { get; set; }
        public string DocumentType { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? Issuer { get; set; }
        public string? Description { get; set; }
        public string? FileUrl { get; set; }
    }
}