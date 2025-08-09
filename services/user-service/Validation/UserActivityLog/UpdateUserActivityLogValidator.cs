using FluentValidation;
using UserService.DTOs.UserActivityLog;

public class UpdateUserActivityLogValidator : AbstractValidator<UpdateUserActivityLogRequest>
{
    public UpdateUserActivityLogValidator()
    {
        RuleFor(x => x.Activity).MaximumLength(100);
    }
}