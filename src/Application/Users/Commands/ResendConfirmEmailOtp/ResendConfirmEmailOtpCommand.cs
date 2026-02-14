using ErrorOr;
using MediatR;

namespace Application.Users.Commands.ResendConfirmEmailOtp;

public record ResendConfirmEmailOtpCommand(
    string Email) : IRequest<ErrorOr<bool>>;
