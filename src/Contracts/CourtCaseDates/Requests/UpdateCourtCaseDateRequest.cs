namespace Contracts.CourtCaseDates.Requests;
public record UpdateCourtCaseDateRequest(
    string Date,
    string Title,
    Guid CaseId);
