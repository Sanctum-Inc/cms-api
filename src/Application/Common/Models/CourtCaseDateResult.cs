namespace Application.Common.Models;
public record CourtCaseDateResult(
    Guid Id,
    string Date,
    string Title,
    Guid CaseId);
