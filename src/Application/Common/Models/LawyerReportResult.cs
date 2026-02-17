using Domain.CourtCases;
using Domain.Invoices;
using Domain.Lawyers;

namespace Application.Common.Models;

public record LawyerReportResult(
    IList<LawyerReportCardsResult> LawyerReportCards,
    IList<LawyerReportInvoicesResult> Invoices,
    IList<LawyerReportCaseDistributionResult> CaseDistributions,
    IList<LawyerReportUpcomingDeadlinesResult> UpcomingDeadlines,
    IList<LawyerReportActivityLogResult> ActivityLog,
    LawyerResult Lawyer);


public record LawyerReportCardsResult(
    string Value,
    string? Description,
    string Percentage);

public record LawyerReportInvoicesResult(
    string InvoiceNumber,
    string CaseName,
    string CaseNumber,
    decimal TotalAmount,
    InvoiceStatus Status);

public record LawyerReportCaseDistributionResult(
    CourtCaseTypes CaseType,
    double TotalAmount
);

public record LawyerReportUpcomingDeadlinesResult(
    string Title,
    string CaseNumber,
    string Date);

public record LawyerReportActivityLogResult(
    string Title,
    string HoursAgo);
