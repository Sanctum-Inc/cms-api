using Application.Document.Commands.Update;
using FluentValidation;

public class UpdateCommandValidator : AbstractValidator<UpdateCommand>
{
    public UpdateCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Document ID must be provided.");

        RuleFor(x => x.FileName)
            .NotEmpty()
            .WithMessage("New name must not be empty.")
            .MaximumLength(250)
            .WithMessage("New name must not exceed 250 characters.");
    }
}
