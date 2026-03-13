using ErrorOr;
using MediatR;

namespace Application.Users.Commands.ForgotPassword;

public record ForgotPasswordCommand(
    string Email) : IRequest<ErrorOr<bool>>;
