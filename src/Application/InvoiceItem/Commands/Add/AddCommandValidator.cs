using FluentValidation;

namespace Application.InvoiceItem.Commands.Add;

public class AddCommandValidator : AbstractValidator<AddCommand>
{
    public AddCommandValidator()
    {
        RuleFor(x => x.InvoiceId)
            .NotEmpty().WithMessage("InvoiceId is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.Hours)
            .GreaterThanOrEqualTo(0).WithMessage("Hours must be zero or positive.");

        RuleFor(x => x.CostPerHour)
            .NotNull().WithMessage("Cost per hour is required.")
            .GreaterThanOrEqualTo(0).WithMessage("Cost per hour must be zero or positive.");
    }
}
