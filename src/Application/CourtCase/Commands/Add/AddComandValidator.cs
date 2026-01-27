using FluentValidation;

namespace Application.CourtCase.Commands.Add;

public class AddCourtCaseCommandValidator : AbstractValidator<AddCommand>
{
    public AddCourtCaseCommandValidator()
    {
        RuleFor(x => x.CaseNumber)
            .NotEmpty().WithMessage("Case number is required.")
            .MaximumLength(100).WithMessage("Case number must not exceed 100 characters.")
            .NotNull();

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required.")
            .MaximumLength(200).WithMessage("Location must not exceed 200 characters.")
            .NotNull();

        RuleFor(x => x.Plaintiff)
            .NotEmpty().WithMessage("Plaintiff is required.")
            .MaximumLength(200).WithMessage("Plaintiff name must not exceed 200 characters.")
            .NotNull();

        RuleFor(x => x.Defendant)
            .NotEmpty().WithMessage("Defendant is required.")
            .MaximumLength(200).WithMessage("Defendant name must not exceed 200 characters.")
            .NotNull();

        RuleFor(x => x.Status)
            .NotNull();

        RuleFor(x => x.Type)
            .MaximumLength(100).WithMessage("Type must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Type))
            .NotNull();
    }
}
