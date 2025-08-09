
namespace UserService.Validation.UserPreference;
using FluentValidation;
using UserService.DTOs.UserPreference;

public class UpdateUserPreferenceValidator : AbstractValidator<UpdateUserPreferenceRequest>
{
    public UpdateUserPreferenceValidator()
    {
        RuleFor(x => x.Language).MaximumLength(20);
        RuleFor(x => x.Theme).MaximumLength(20);
    }
}
