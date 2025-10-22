using FluentValidation;

namespace Application.InvoiceItem.Commands.Update
{
    public class UpdateCommandValidator : AbstractValidator<UpdateCommand>
    {
        public UpdateCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(x => x.InvoiceId)
                .NotEmpty().WithMessage("InvoiceId is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

            RuleFor(x => x.Hours)
                .GreaterThanOrEqualTo(0).WithMessage("Hours must be zero or positive.");

            RuleFor(x => x.CaseId)
                .NotEmpty().WithMessage("CaseId is required.");

            When(x => x.IsDayFee, () =>
            {
                RuleFor(x => x.DayFeeAmount)
                    .NotNull().WithMessage("Day fee amount is required when IsDayFee is true.")
                    .GreaterThanOrEqualTo(0).WithMessage("Day fee amount must be zero or positive.");

                RuleFor(x => x.CostPerHour)
                    .Must(x => x == null)
                    .WithMessage("Cost per hour must be null when IsDayFee is true.");
            });

            When(x => !x.IsDayFee, () =>
            {
                RuleFor(x => x.CostPerHour)
                    .NotNull().WithMessage("Cost per hour is required when IsDayFee is false.")
                    .GreaterThanOrEqualTo(0).WithMessage("Cost per hour must be zero or positive.");

                RuleFor(x => x.DayFeeAmount)
                    .Must(x => x == null)
                    .WithMessage("Day fee amount must be null when IsDayFee is false.");
            });
        }
    }
}
