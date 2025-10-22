using ErrorOr;
using MediatR;

namespace Application.Document.Commands.Update;
public record UpdateCommand(
    Guid Id,
    string FileName) : IRequest<ErrorOr<bool>>;
