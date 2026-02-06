using Domain.CourtCaseDates;

namespace Contracts.CourtCaseDates.Requests;

public record UpdateCourtCaseDateRequest(
    string Date,
    string Title,
    string Description,
    CourtCaseDateTypes Type,
    bool IsComplete,
    bool IsCancelled,
    Guid CaseId);
