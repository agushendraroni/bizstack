
using System.ComponentModel.DataAnnotations;
using SharedLibrary.Entities;

namespace OrganizationService.Models  {
    public class Company : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

         public string? Address { get; set; }


    }
}