using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Models;
using Domain.CourtCases;
using ErrorOr;

namespace Infrastructure.Services;

public class DashboardService : IDashboardService
{
    private ICourtCaseRepository _courtCaseRepository;

    public DashboardService(ICourtCaseRepository courtCaseRepository)
    {
        _courtCaseRepository = courtCaseRepository;
    }

    public async Task<ErrorOr<DashBoardResult>> GetDashboardInformation(CancellationToken cancellationToken)
    {
        var courtCases = await _courtCaseRepository.GetAll(cancellationToken);

        var courtCaseArray = courtCases as CourtCase[] ?? courtCases.ToArray();
        int totalCases = courtCaseArray.Count();
        decimal totalInvoices =
            courtCaseArray
                .SelectMany(c => c.Invoices)
                .SelectMany(i => i.Items)
                .Sum(item => item.CostPerHour * item.Hours);
        int totalUpcomingDates =
            courtCaseArray
                .SelectMany(c => c.CourtCaseDates)
                .Count(d => DateTime.Parse(d.Date).Date > DateTime.Today);
        int totalDocumentsStored =
            courtCaseArray
                .SelectMany(c => c.Documents)
                .Count();

        var recentActivityCases = courtCaseArray
            .Select(x => new RecentCaseActivityResult(
                $"Case {x.CaseNumber} has been added.",
                $" {x.Type} | {x.Plaintiff} vs {x.Defendant}",
                x.Created.ToString("yyyy-MM-dd HH:mm:ss"),
                GetTimeAgo(x.Created)));

        var recentActivityInvoices = courtCaseArray
            .SelectMany(c => c.Invoices)
            .Select(x => new RecentCaseActivityResult(
                $"Invoice {x.InvoiceNumber} created",
                $" {x.Status} | Issued to: {x.ClientName}",
                x.Created.ToString("yyyy-MM-dd HH:mm:ss"),
                GetTimeAgo(x.Created)));

        var recentActivityDocuments = courtCaseArray
            .SelectMany(c => c.Documents)
            .Select(x => new RecentCaseActivityResult(
                $"File {x.FileName} has been added.",
                $" {x.Created} | For case: {x.Case?.CaseNumber}",
                x.Created.ToString("yyyy-MM-dd HH:mm:ss"),
                GetTimeAgo(x.Created)));

        var recentActivity = recentActivityCases
            .Concat(recentActivityDocuments)
            .Concat(recentActivityInvoices);



        var upcomingCases = courtCaseArray
            .SelectMany(c => c.CourtCaseDates)
            .Select(x => new UpcomingCourtCaseActivityResult(
                $"{x.Date}",
                $"{x.Case.CaseNumber}: {x.Case.Defendant} vs {x.Case.Plaintiff}",
                x.Created.ToString("yyyy-MM-dd HH:mm:ss"),
                x.Type));


        return new DashBoardResult(
            totalCases,
            totalInvoices,
            totalUpcomingDates,
            totalDocumentsStored,
            recentActivity.OrderByDescending(x => x.Date)
                .ToList()
                .Take(15),
            upcomingCases.OrderByDescending(x => x.Date)
                .ToList()
                .Take(15));
    }

    private static string GetTimeAgo(DateTime createdDate)
    {
        var now = DateTime.Now;
        var diff = now - createdDate;

        if (diff.TotalSeconds < 60)
            return "just now";

        if (diff.TotalMinutes < 60)
        {
            int minutes = (int)diff.TotalMinutes;
            return minutes == 1 ? "1 minute ago" : $"{minutes} minutes ago";
        }

        if (diff.TotalHours < 24)
        {
            int hours = (int)diff.TotalHours;
            return hours == 1 ? "1 hour ago" : $"{hours} hours ago";
        }

        if (diff.TotalDays < 30)
        {
            int days = (int)diff.TotalDays;
            return days == 1 ? "1 day ago" : $"{days} days ago";
        }

        if (diff.TotalDays < 365)
        {
            int months = (int)(diff.TotalDays / 30);
            return months == 1 ? "1 month ago" : $"{months} months ago";
        }

        int years = (int)(diff.TotalDays / 365);
        return years == 1 ? "1 year ago" : $"{years} years ago";
    }

}
