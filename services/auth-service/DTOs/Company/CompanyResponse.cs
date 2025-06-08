namespace AuthService.DTOs.Company
{
    public class CompanyResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? ChangedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? ChangedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
