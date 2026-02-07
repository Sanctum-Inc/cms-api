using MediatR;
using ErrorOr;

namespace Application.CourtCaseDates.Commands.SetToComplete;

public record SetToCompleteCommand(
    Guid Id) : IRequest<ErrorOr<bool>>;
