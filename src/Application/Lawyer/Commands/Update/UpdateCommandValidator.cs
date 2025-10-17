using FluentValidation;

namespace Application.Lawyer.Commands.Update;
public class UpdateCommandValidator : AbstractValidator<UpdateCommand>
{
    public UpdateCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Surname)
            .NotEmpty().WithMessage("Surname is required.")
            .MaximumLength(100).WithMessage("Surname must not exceed 100 characters.");

        RuleFor(x => x.MobileNumber)
            .NotEmpty().WithMessage("Mobile number is required.")
            .Matches(@"^\+?\d{7,15}$").WithMessage("Invalid mobile number format.");

        RuleFor(x => x.Specialty)
            .IsInEnum().WithMessage("Specialty must be a valid value.");
    }
}
