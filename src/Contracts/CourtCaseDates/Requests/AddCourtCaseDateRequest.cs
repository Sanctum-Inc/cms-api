using Domain.CourtCaseDates;

namespace Contracts.CourtCaseDates.Requests;
public record AddCourtCaseDateRequest(
    string Date,
    string Title,
    Guid CaseId,
    CourtCaseDateTypes Type
);
