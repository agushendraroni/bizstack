using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrganizationService.Models  
{
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TenantId { get; set; }
        
        // CompanyId sama dengan TenantId untuk foreign key references
        [NotMapped]
        public int CompanyId => TenantId;

        [Required, MaxLength(50)]
        public string Code { get; set; } = null!;
        
        [Required, MaxLength(200)]
        public string Name { get; set; } = null!;
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        [MaxLength(300)]
        public string? Address { get; set; }
        
        [MaxLength(20)]
        public string? Phone { get; set; }
        
        [MaxLength(100)]
        public string? Email { get; set; }
        
        [MaxLength(200)]
        public string? Website { get; set; }
        
        [MaxLength(20)]
        public string? NPWP { get; set; }
        
        [MaxLength(20)]
        public string? NIB { get; set; }
        
        [MaxLength(20)]
        public string? SIUP { get; set; }

        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        // Navigation properties
        public BusinessGroup? BusinessGroup { get; set; }
        public ICollection<Branch> Branches { get; set; } = new List<Branch>();
        public ICollection<LegalDocument> LegalDocuments { get; set; } = new List<LegalDocument>();
        public ICollection<JobTitle> JobTitles { get; set; } = new List<JobTitle>();
    }
}
