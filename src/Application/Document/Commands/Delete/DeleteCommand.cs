using ErrorOr;
using MediatR;

namespace Application.Document.Commands.Delete;

public record DeleteCommand(Guid Id) : IRequest<ErrorOr<bool>>;
