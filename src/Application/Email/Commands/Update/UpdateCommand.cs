using Domain.Emails;
using ErrorOr;
using MediatR;

namespace Application.Email.Commands.Update;
public record UpdateCommand(
    Guid Id,
    EmailStatus Status) : IRequest<ErrorOr<bool>>;
