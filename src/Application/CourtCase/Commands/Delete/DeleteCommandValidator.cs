using FluentValidation;

namespace Application.CourtCase.Commands.Delete;

public class DeleteCommandValidator : AbstractValidator<DeleteCommand>
{
    public DeleteCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty()
            .NotNull();

    }
}
