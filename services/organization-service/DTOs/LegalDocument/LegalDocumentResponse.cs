namespace OrganizationService.DTOs.LegalDocument
{
    public class LegalDocumentResponse
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string DocumentType { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? Issuer { get; set; }
        public string? Description { get; set; }
        public string? FileUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ChangedAt { get; set; }
        public string? ChangedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}