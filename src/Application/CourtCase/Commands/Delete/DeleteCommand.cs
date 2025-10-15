using ErrorOr;
using MediatR;

namespace Application.CourtCase.Commands.Delete;
public record DeleteCommand(
    string Id) : IRequest<ErrorOr<bool>>;