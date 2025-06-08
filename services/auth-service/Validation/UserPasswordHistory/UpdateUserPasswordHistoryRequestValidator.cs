using AuthService.DTOs.UserPasswordHistory;
using FluentValidation;

namespace AuthService.Validation.UserPasswordHistory
{
    public class UpdateUserPasswordHistoryRequestValidator : AbstractValidator<UpdateUserPasswordHistoryRequest>
    {
        public UpdateUserPasswordHistoryRequestValidator()
        {
            RuleFor(x => x.PasswordHash)
                .NotEmpty().WithMessage("PasswordHash wajib diisi.")
                .MinimumLength(32).WithMessage("PasswordHash minimal 32 karakter (hash).");
        }
    }
}