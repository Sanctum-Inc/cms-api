using Domain.CourtCases;
using ErrorOr;
using MediatR;

namespace Application.CourtCase.Commands.Add;

public record AddCommand(
    string CaseNumber,
    string Location,
    string Plaintiff,
    string Defendant,
    CourtCaseStatus Status,
    CourtCaseTypes Type,
    CourtCaseOutcomes Outcome) : IRequest<ErrorOr<Guid>>;
