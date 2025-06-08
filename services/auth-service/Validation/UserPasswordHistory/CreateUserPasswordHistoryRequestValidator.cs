using AuthService.DTOs.UserPasswordHistory;
using FluentValidation;

namespace AuthService.Validation.UserPasswordHistory
{
    public class CreateUserPasswordHistoryRequestValidator : AbstractValidator<CreateUserPasswordHistoryRequest>
    {
        public CreateUserPasswordHistoryRequestValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId harus lebih besar dari 0.");

            RuleFor(x => x.PasswordHash)
                .NotEmpty().WithMessage("PasswordHash wajib diisi.")
                .MinimumLength(32).WithMessage("PasswordHash minimal 32 karakter (hash).");
        }
    }
}