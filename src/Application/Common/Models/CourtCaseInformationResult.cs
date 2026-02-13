using Domain.CourtCaseDates;
using Domain.CourtCases;
using Domain.Invoices;

namespace Application.Common.Models;

public class CourtCaseInformationResult
{
    public required Guid CaseId { get; set; }
    public required string CaseNumber { get; set; }
    public required string Location { get; set; }
    public required string Plaintiff { get; set; }
    public required string Defendant { get; set; }
    public required CourtCaseTypes CaseType { get; set; }
    public required CourtCaseOutcomes CaseOutcomes { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime LastModified { get; set; }
    public required IList<CourtCaseInformationQueryDatesResult> Dates { get; set; }
    public required IList<CourtCaseInformationQueryDocumentsResult> Documents { get; set; }
    public required IList<CourtCaseInformationQueryInvoicesResult> Invoices { get; set; }
    public required IList<CourtCaseInformationQueryLawyersResult> Lawyers { get; set; }
}

public class CourtCaseInformationQueryDatesResult
{
    public required DateTime Date { get; set; }
    public required CourtCaseDateTypes DateType { get; set; }
    public required string Description { get; set; }
}

public class CourtCaseInformationQueryDocumentsResult
{
    public required string Title { get; set; }
    public required string FileType { get; set; }
    public required DateTime DateFiled { get; set; }
}

public class CourtCaseInformationQueryInvoicesResult
{
    public required string InvoiceNumber { get; set; }
    public required decimal Amount { get; set; }
    public required InvoiceStatus Status { get; set; }
}

public class CourtCaseInformationQueryLawyersResult
{
    public required string Name { get; set; }
    public required string MobileNumber { get; set; }
    public required string Email { get; set; }
}



