using Contracts.CourtCaseDates.Responses;
using Contracts.Documents.Responses;
using Contracts.Invoice.Responses;
using Contracts.Lawyer.Responses;
using Domain.CourtCases;

namespace Contracts.CourtCases.Responses;

public class CourtCasesResponse
{
    public required Guid Id { get; set; }
    public required string CaseNumber { get; set; }
    public required string Location { get; set; }
    public required string Plaintiff { get; set; }
    public required string Defendant { get; set; }
    public required CourtCaseStatus Status { get; set; }
    public required CourtCaseTypes Type { get; set; }
    public CourtCaseOutcomes Outcome { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }
    public List<CourtCaseDateResponse> CourtCaseDates { get; set; } = [];
    public List<DocumentResponse> Documents { get; set; } = [];
    public List<InvoiceResponse> Invoices { get; set; } = [];
    public List<LawyerResponse> Lawyers { get; set; } = [];
}
