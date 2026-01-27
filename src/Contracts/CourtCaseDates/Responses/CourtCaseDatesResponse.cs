using Contracts.CourtCases.Responses;

namespace Contracts.CourtCaseDates.Responses;
public record CourtCaseDatesResponse(
    Guid Id,
    string Date,
    string Title,
    string Type,
    string CaseNumber,
    Guid CaseId,
    string CaseType,
    string Platiniff,
    string Defendent);
