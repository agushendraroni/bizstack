using System.ComponentModel.DataAnnotations;

namespace AuthService.DTOs.User
{
    /// <summary>
    /// Request DTO untuk membuat user baru.
    /// </summary>
    public class CreateUserRequest
    {
        [Required(ErrorMessage = "Username wajib diisi.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Username harus antara 3 sampai 100 karakter.")]
        [RegularExpression(@"^[a-zA-Z0-9_.-]+$", ErrorMessage = "Username hanya boleh huruf, angka, titik, underscore, dan strip.")]
        public string Username { get; set; } = default!;

        [Required(ErrorMessage = "CompanyId wajib diisi.")]
        [Range(1, int.MaxValue, ErrorMessage = "CompanyId harus lebih besar dari 0.")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "PasswordHash wajib diisi.")]
        [MinLength(32, ErrorMessage = "PasswordHash minimal 32 karakter (hash).")]
        [MaxLength(128, ErrorMessage = "PasswordHash maksimal 128 karakter.")]
        public string PasswordHash { get; set; } = default!;
    }
}
