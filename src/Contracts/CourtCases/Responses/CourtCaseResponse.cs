using Domain.CourtCases;

namespace Contracts.CourtCases.Responses;

public record CourtCaseResponse
(
    Guid Id,
    string CaseNumber,
    CourtCaseStatus Status,
    string Location,
    string NextDate,
    string Plaintiff,
    CourtCaseTypes Type
);

