using ErrorOr;
using MediatR;

namespace Application.Document.Commands.Update;
public record UpdateCommand(
    Guid Id,
    string NewName) : IRequest<ErrorOr<bool>>;