using ErrorOr;
using MediatR;

namespace Application.CourtCase.Commands.Delete;
public record DeleteCommand(
    Guid Id) : IRequest<ErrorOr<bool>>;