using Domain.CourtCases;

namespace Application.Common.Models;
public record CourtCaseDateResult(
    Guid Id,
    string Date,
    string Title,
    string CaseNumber,
    Guid CaseId,
    CourtCaseTypes CaseType,
    string Platiniff,
    string Defendent);
