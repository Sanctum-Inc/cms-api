using MediatR;
using ErrorOr;

namespace Application.CourtCaseDates.Commands.SetToCancelled;

public record SetToCompleteCommand(
    Guid Id) : IRequest<ErrorOr<bool>>;
