using ErrorOr;
using MediatR;

namespace Application.Users.Commands.ConfirmEmailOtp;

public record ConfirmEmailOtpCommand(
    string Token,
    string Email) : IRequest<ErrorOr<string>>;
