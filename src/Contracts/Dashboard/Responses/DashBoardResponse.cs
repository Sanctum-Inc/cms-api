using Domain.CourtCaseDates;

namespace Contracts.Dashboard.Responses;

public record DashBoardResponse(
    int TotalCases,
    decimal TotalInvoices,
    int UpcomingDates,
    int DocumentsStored,
    IEnumerable<RecentCaseActivityResponse>  RecentCases,
    IEnumerable<UpcomingCourtCaseActivityResponse>  UpcomingCases);

public record RecentCaseActivityResponse(
    string ActitvityTitle,
    string ActivityDescription,
    string Date,
    string TimeSince);

public record UpcomingCourtCaseActivityResponse(
    string CourtDateTitle,
    string CourtDateDescription,
    string Date,
    CourtCaseDateTypes CourtDateType);
