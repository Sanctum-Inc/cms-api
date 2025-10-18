namespace Contracts.CourtCases.Requests;
public record AddCourtCaseRequest(
        string CaseNumber,
        string Location,
        string Plaintiff,
        string Defendant,
        string Status,
        string? Type,
        string? Outcome);
