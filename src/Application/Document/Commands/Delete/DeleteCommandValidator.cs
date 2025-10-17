using FluentValidation;

namespace Application.Document.Commands.Delete;
public class DeleteCommandValidator : AbstractValidator<DeleteCommand>
{
    public DeleteCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("A case Id must be provided.")
            .NotNull().WithMessage("A case Id must be provided.");
    }
}

