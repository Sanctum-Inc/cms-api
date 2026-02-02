using ErrorOr;
using MediatR;

namespace Application.Lawyer.Commands.Delete;

public record DeleteCommand(
    Guid Id) : IRequest<ErrorOr<bool>>;
