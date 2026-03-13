using FluentValidation;

namespace Application.Users.Commands.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Token cannot be null or empty")
            .NotNull()
            .WithMessage("Token cannot be null or empty");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password cannot be null or empty")
            .NotNull()
            .WithMessage("Password cannot be null or empty");
    }
}
