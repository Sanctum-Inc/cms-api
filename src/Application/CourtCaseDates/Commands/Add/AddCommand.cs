using Domain.CourtCaseDates;
using ErrorOr;
using MediatR;

namespace Application.CourtCaseDates.Commands.Add;

public record AddCommand(
    string Date,
    string Title,
    string Description,
    Guid CaseId,
    CourtCaseDateTypes Type
) : IRequest<ErrorOr<Guid>>;
