using FluentValidation;

namespace Application.CourtCase.Commands.Update;
public class UpdateCommandValidator : AbstractValidator<UpdateCommand>
{
    public UpdateCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.")
            .NotNull().WithMessage("Id cannot be null.");

        RuleFor(x => x.CaseNumber)
            .NotEmpty().WithMessage("Case number is required.")
            .NotNull().WithMessage("Case number must not be null")
            .MaximumLength(100).WithMessage("Case number must not exceed 100 characters.");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required.")
            .NotNull().WithMessage("Location must not be null")
            .MaximumLength(200).WithMessage("Location must not exceed 200 characters.");

        RuleFor(x => x.Plaintiff)
            .NotEmpty().WithMessage("Plaintiff is required.")
            .NotNull().WithMessage("Plaintiff must not be null")
            .MaximumLength(200).WithMessage("Plaintiff name must not exceed 200 characters.");

        RuleFor(x => x.Defendant)
            .NotEmpty().WithMessage("Defendant is required.")
            .NotNull().WithMessage("Defendant must not be null")
            .MaximumLength(200).WithMessage("Defendant name must not exceed 200 characters.");

        RuleFor(x => x.Status)
            .NotNull().WithMessage("Status must not be null");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required.")
            .NotNull().WithMessage("Type must not be null")
            .MaximumLength(100).WithMessage("Type must not exceed 100 characters.");

        RuleFor(x => x.Outcome)
            .NotEmpty().WithMessage("Outcome is required.")
            .NotNull().WithMessage("Outcome must not be null")
            .MaximumLength(500).WithMessage("Outcome must not exceed 500 characters.");




    }
}
