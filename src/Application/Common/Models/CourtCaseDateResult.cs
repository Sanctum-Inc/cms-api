using Domain.CourtCaseDates;

namespace Application.Common.Models;

public record CourtCaseDateResult(
    int OverdueItems,
    int CompletionRate,
    int UpcomingEvents,
    float ChangeFromLastMonth,
    CourtCaseDateItem? DeadlineCase,
    IEnumerable<CourtCaseDateItem> CourtCaseDateItems);

public record CourtCaseDateItem(
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
