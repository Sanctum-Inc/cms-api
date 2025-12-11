namespace Contracts.CourtCaseDates.Responses;
public record CourtCaseDatesResponse(
    Guid Id,
    string Date,
    string Title,
    string Type,
    Guid CaseId);
