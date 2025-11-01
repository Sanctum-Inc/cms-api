namespace Contracts.CourtCaseDates.Responses;
public record CourtCaseDatesResponse(
    Guid Id,
    string Date,
    string Title,
    Guid CaseId);
