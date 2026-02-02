using FluentValidation;

namespace Application.Lawyer.Commands.Delete;

public class DeleteCommandValidator : AbstractValidator<DeleteCommand>
{
    public DeleteCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("Lawyer ID must not be null.")
            .NotEmpty().WithMessage("Lawyer ID must not be empty.");
    }
}
