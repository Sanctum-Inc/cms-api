using Domain.CourtCaseDates;
using ErrorOr;
using MediatR;

namespace Application.CourtCaseDates.Commands.Update;

public record UpdateCommand(
    Guid Id,
    string Date,
    string Title,
    string Description,
    bool IsComplete,
    bool IsCancelled,
    CourtCaseDateTypes Type,
    Guid CaseId
) : IRequest<ErrorOr<bool>>;
