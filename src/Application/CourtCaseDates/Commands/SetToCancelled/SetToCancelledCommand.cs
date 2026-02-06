using MediatR;
using ErrorOr;

namespace Application.CourtCaseDates.Commands.SetToCancelled;

public record SetToCancelledCommand(
    Guid Id) : IRequest<ErrorOr<bool>>;
