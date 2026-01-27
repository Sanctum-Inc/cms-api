namespace Application.Common.Models;
public record CourtCaseDateResult(
    Guid Id,
    string Date,
    string Title,
    string Type,
    string CaseNumber,
    Guid CaseId,
    string CaseType,
    string Platiniff,
    string Defendent);
