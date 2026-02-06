using FluentValidation;

namespace Application.CourtCaseDates.Commands.SetToCancelled;

public class SetToCancelledCommandValidator : AbstractValidator<SetToCancelledCommand>
{
    public  SetToCancelledCommandValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty()
            .WithMessage("The id cannot be empty")
            .NotNull()
            .WithMessage("The id cannot be null");
    }
}
