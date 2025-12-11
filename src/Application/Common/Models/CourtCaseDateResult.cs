namespace Application.Common.Models;
public record CourtCaseDateResult(
    Guid Id,
    string Date,
    string Title,
    string Type,
    Guid CaseId);
