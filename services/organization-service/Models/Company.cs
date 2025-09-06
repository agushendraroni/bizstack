using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace OrganizationService.Models  
{
    public class Company : BaseEntity
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? NPWP { get; set; }
        public string? NIB { get; set; }
        public string? SIUP { get; set; }

        public BusinessGroup? BusinessGroup { get; set; }

        public ICollection<Branch> Branches { get; set; } = new List<Branch>();
        public ICollection<LegalDocument> LegalDocuments { get; set; } = new List<LegalDocument>();
        public ICollection<JobTitle> JobTitles { get; set; } = new List<JobTitle>();
    }
}
