using Application.Document.Commands.Add;
using FluentValidation;

public class AddCommandValidator : AbstractValidator<AddCommand>
{
    public AddCommandValidator()
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage("A file must be provided.")
            .Must(f => f.Length > 0).WithMessage("The file cannot be empty.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("A name must be provided.")
            .MaximumLength(255).WithMessage("The name cannot exceed 255 characters.");

        RuleFor(x => x.CaseId)
            .NotEmpty().WithMessage("A case Id must be provided.")
            .MaximumLength(255).WithMessage("The case Id cannot exceed 255 characters.");
    }
}
