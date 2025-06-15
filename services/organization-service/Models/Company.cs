
using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace OrganizationService.Models  {
    public class Company : BaseEntity
    {
         public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string NPWP { get; set; } = null!;
        public string? NIB { get; set; }
        public string? SIUP { get; set; }
        public string Address { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }

        public Guid? BusinessGroupId { get; set; }
        public BusinessGroup? BusinessGroup { get; set; }

        public ICollection<Branch> Branches { get; set; } = new List<Branch>();
        public ICollection<LegalDocument> LegalDocuments { get; set; } = new List<LegalDocument>();
        public ICollection<JobTitle> JobTitles { get; set; } = new List<JobTitle>();



    }
}