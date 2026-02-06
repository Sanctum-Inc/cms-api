using FluentValidation;

namespace Application.CourtCaseDates.Commands.SetToCancelled;

public class SetToCompleteCommandValidator : AbstractValidator<SetToCancelledCommand>
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
