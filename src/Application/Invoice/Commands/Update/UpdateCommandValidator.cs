using FluentValidation;

namespace Application.Invoice.Commands.Update
{
    public class UpdateCommandValidator : AbstractValidator<UpdateCommand>
    {
        public UpdateCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Invoice ID is required.");

            RuleFor(x => x.InvoiceNumber)
                .NotEmpty().WithMessage("Invoice number is required.")
                .MaximumLength(50);

            RuleFor(x => x.InvoiceDate)
                .NotEmpty().WithMessage("Invoice date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Invoice date cannot be in the future.");

            RuleFor(x => x.ClientName)
                .NotEmpty().WithMessage("Client name is required.")
                .MaximumLength(200);

            RuleFor(x => x.Reference)
                .NotEmpty().WithMessage("Reference is required.")
                .MaximumLength(200);

            RuleFor(x => x.CaseName)
                .NotEmpty().WithMessage("Case name is required.")
                .MaximumLength(300);

            RuleFor(x => x.AccountName)
                .NotEmpty().WithMessage("Account name is required.")
                .MaximumLength(200);

            RuleFor(x => x.Bank)
                .NotEmpty().WithMessage("Bank name is required.")
                .MaximumLength(100);

            RuleFor(x => x.BranchCode)
                .NotEmpty().WithMessage("Branch code is required.")
                .MaximumLength(50);

            RuleFor(x => x.AccountNumber)
                .NotEmpty().WithMessage("Account number is required.")
                .MaximumLength(50);
        }
    }
}
