using Domain.CourtCases;

namespace Contracts.CourtCases.Requests;

public record AddCourtCaseRequest(
    string CaseNumber,
    string Location,
    string Plaintiff,
    string Defendant,
    CourtCaseStatus Status,
    CourtCaseTypes Type,
    CourtCaseOutcomes Outcome);
