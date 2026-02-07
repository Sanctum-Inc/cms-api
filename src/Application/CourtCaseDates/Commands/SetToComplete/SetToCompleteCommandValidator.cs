using Application.CourtCaseDates.Commands.SetToCancelled;
using FluentValidation;

namespace Application.CourtCaseDates.Commands.SetToComplete;

public class SetToCompleteCommandValidator : AbstractValidator<SetToCompleteCommand>
{
    public  SetToCompleteCommandValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty()
            .WithMessage("The id cannot be empty")
            .NotNull()
            .WithMessage("The id cannot be null");
    }
}
