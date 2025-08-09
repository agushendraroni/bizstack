using FluentValidation;
using UserService.DTOs.UserPreference;

namespace UserService.Validation;

public class CreateUserPreferenceValidator : AbstractValidator<CreateUserPreferenceRequest>
{
    public CreateUserPreferenceValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Language).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Theme).NotEmpty().MaximumLength(20);
    }
}
