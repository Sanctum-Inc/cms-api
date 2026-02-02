using FluentValidation;

namespace Application.Email.Commands.Add;

public class AddCommandValidator : AbstractValidator<AddCommand>
{
    public AddCommandValidator()
    {
        RuleFor(x => x.To)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Subject)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Body)
            .NotNull()
            .NotEmpty();
    }
}
