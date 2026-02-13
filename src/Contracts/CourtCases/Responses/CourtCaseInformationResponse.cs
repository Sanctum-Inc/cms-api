using Domain.CourtCaseDates;
using Domain.CourtCases;
using Domain.Invoices;

namespace Contracts.CourtCases.Responses;

public class CourtCaseInformationResponse
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
    public required IList<CourtCaseInformationQueryDatesResponse> Dates { get; set; }
    public required IList<CourtCaseInformationQueryDocumentsResponse> Documents { get; set; }
    public required IList<CourtCaseInformationQueryInvoicesResponse> Invoices { get; set; }
    public required IList<CourtCaseInformationQueryLawyersResponse> Lawyers { get; set; }
}

public class CourtCaseInformationQueryDatesResponse
{
    public required DateTime Date { get; set; }
    public required CourtCaseDateTypes DateType { get; set; }
    public required string Description { get; set; }
}

public class CourtCaseInformationQueryDocumentsResponse
{
    public required string Title { get; set; }
    public required string FileType { get; set; }
    public required DateTime DateFiled { get; set; }
}

public class CourtCaseInformationQueryInvoicesResponse
{
    public required string InvoiceNumber { get; set; }
    public required decimal Amount { get; set; }
    public required InvoiceStatus Status { get; set; }
}

public class CourtCaseInformationQueryLawyersResponse
{
    public required string Name { get; set; }
    public required string MobileNumber { get; set; }
    public required string Email { get; set; }
}
