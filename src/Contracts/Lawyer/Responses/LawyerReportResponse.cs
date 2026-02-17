using Domain.CourtCases;
using Domain.Invoices;

namespace Contracts.Lawyer.Responses;

public record LawyerReportResponse(
    IList<LawyerReportCardsResponse> LawyerReportCards,
    IList<LawyerReportInvoicesResponse> Invoices,
    IList<LawyerReportCaseDistributionResponse> CaseDistributions,
    IList<LawyerReportUpcomingDeadlinesResponse> UpcomingDeadlines,
    IList<LawyerReportActivityLogResponse> ActivityLog,
    LawyerResponse Lawyer);


public record LawyerReportCardsResponse(
    string Value,
    string? Description,
    string Percentage);

public record LawyerReportInvoicesResponse(
    string InvoiceNumber,
    string CaseName,
    string CaseNumber,
    decimal TotalAmount,
    InvoiceStatus Status);

public record LawyerReportCaseDistributionResponse(
    CourtCaseTypes CaseType,
    double TotalAmount
);

public record LawyerReportUpcomingDeadlinesResponse(
    string Title,
    string CaseNumber,
    string Date);

public record LawyerReportActivityLogResponse(
    string Title,
    string HoursAgo);
