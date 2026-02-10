using Domain.CourtCaseDates;

namespace Application.Common.Models;

public record DashBoardResult(
    int TotalCases,
    decimal TotalInvoices,
    int UpcomingDates,
    int DocumentsStored,
    IEnumerable<RecentCaseActivityResult>  RecentCases,
    IEnumerable<UpcomingCourtCaseActivityResult>  UpcomingCases);

public record RecentCaseActivityResult(
    string ActitvityTitle,
    string ActivityDescription,
    string Date,
    string TimeSince);

public record UpcomingCourtCaseActivityResult(
    string CourtDateTitle,
    string CourtDateDescription,
    string Date,
    CourtCaseDateTypes CourtDateType,
    Guid Id);
