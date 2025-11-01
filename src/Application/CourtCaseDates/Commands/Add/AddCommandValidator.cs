using FluentValidation;

namespace Application.CourtCaseDates.Commands.Add;
public class AddCommandValidator : AbstractValidator<AddCommand>
{
    public AddCommandValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200);

        RuleFor(x => x.CaseId)
            .NotEmpty().WithMessage("CaseId is required.");
    }
}
