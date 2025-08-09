using FluentValidation;
using UserService.DTOs.UserActivityLog;

namespace UserService.Validation;

public class CreateUserActivityLogValidator : AbstractValidator<CreateUserActivityLogRequest>
{
    public CreateUserActivityLogValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Activity).NotEmpty().MaximumLength(100);
    }
}
