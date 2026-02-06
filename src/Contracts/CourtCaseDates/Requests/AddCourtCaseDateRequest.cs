using Domain.CourtCaseDates;

namespace Contracts.CourtCaseDates.Requests;

public record AddCourtCaseDateRequest(
    string Date,
    string Title,
    string Description,
    Guid CaseId,
    CourtCaseDateTypes Type
);
