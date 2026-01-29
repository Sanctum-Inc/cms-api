using ErrorOr;
using MediatR;

namespace Application.Users.Commands.Register;
public record RegisterCommand(
    string Email,
    string Name,
    string Surname,
    string MobileNumber,
    string Password,
    string FirmId) : IRequest<ErrorOr<bool>>;

