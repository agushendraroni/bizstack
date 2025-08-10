namespace OrganizationService.DTOs.LegalDocument
{
    public class UpdateLegalDocumentRequest
    {
        public string? DocumentType { get; set; }
        public string? DocumentNumber { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? Issuer { get; set; }
        public string? Description { get; set; }
        public string? FileUrl { get; set; }
    }
}