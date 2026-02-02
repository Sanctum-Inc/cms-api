using Domain.CourtCases;
using ErrorOr;
using MediatR;

namespace Application.CourtCase.Commands.Update;

public record UpdateCommand(
    Guid Id,
    string CaseNumber,
    string Location,
    string Plaintiff,
    string Defendant,
    CourtCaseStatus Status,
    CourtCaseTypes Type,
    CourtCaseOutcomes Outcome) : IRequest<ErrorOr<bool>>;
