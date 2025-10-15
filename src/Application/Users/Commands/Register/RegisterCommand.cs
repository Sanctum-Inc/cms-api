using ErrorOr;
using MediatR;

namespace Application.Users.Commands.Register;
public record RegisterCommand(
    string Email,
    string Name,
    string Surname,
    string MobileNumber,
    string Password) : IRequest<ErrorOr<bool>>;

