using Domain.CourtCaseDates;

namespace Contracts.CourtCaseDates.Responses;

public record CourtCaseDateResponse(
    int OverdueItems,
    int CompletionRate,
    int UpcomingEvents,
    float ChangeFromLastMonth,
    CourtCaseDateItemResponse DeadlineCase,
    IEnumerable<CourtCaseDateItemResponse> CourtCaseDateItems);

public record CourtCaseDateItemResponse(
    Guid Id,
    string Date,
    string Title,
    string CaseNumber,
    Guid CaseId,
    CourtCaseDateTypes CourtCaseDateType,
    string Subtitle,
    string Description,
    string Status
);
