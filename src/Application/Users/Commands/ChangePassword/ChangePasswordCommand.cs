using ErrorOr;
using MediatR;

namespace Application.Users.Commands.ChangePassword;

public record ChangePasswordCommand(
    string Token,
    string Password) : IRequest<ErrorOr<bool>>;
