using System.ComponentModel.DataAnnotations;
using AuthService.DTOs.Common;

namespace AuthService.DTOs.User
{
    /// <summary>
    /// Filter untuk pencarian user (dengan pagination).
    /// </summary>
    public class UserFilterRequest : PaginationFilter
    {
        [StringLength(100, ErrorMessage = "Username maksimal 100 karakter.")]
        [RegularExpression(@"^[a-zA-Z0-9_.-]*$", ErrorMessage = "Username hanya boleh huruf, angka, titik, underscore, dan strip.")]
        public string? Username { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "CompanyId harus lebih besar dari 0.")]
        public int? CompanyId { get; set; }
    }
}
