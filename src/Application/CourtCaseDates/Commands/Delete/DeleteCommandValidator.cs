using FluentValidation;

namespace Application.CourtCaseDates.Commands.Delete;
public class DeleteCommandValidator : AbstractValidator<DeleteCommand>
{
    public DeleteCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("Id is required.");
    }
}
