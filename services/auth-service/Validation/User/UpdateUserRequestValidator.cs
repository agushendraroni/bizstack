using AuthService.DTOs.User;
using FluentValidation;

namespace AuthService.Validation.User
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username wajib diisi.")
                .Length(3, 100).WithMessage("Username harus antara 3 sampai 100 karakter.")
                .Matches(@"^[a-zA-Z0-9_.-]+$").WithMessage("Username hanya boleh huruf, angka, titik, underscore, dan strip.");

            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage("CompanyId harus lebih besar dari 0.");

            // Password optional, tapi jika diisi harus minimal 6 karakter
            When(x => !string.IsNullOrEmpty(x.Password), () =>
            {
                RuleFor(x => x.Password)
                    .MinimumLength(6).WithMessage("Password minimal 6 karakter.");
            });
        }
    }
}