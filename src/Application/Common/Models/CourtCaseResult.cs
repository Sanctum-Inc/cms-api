using Domain.CourtCaseDates;
using Domain.CourtCases;
using Domain.InvoiceItems;
using Domain.Invoices;
using Domain.Users;

namespace Application.Common.Models;
public class CourtCaseResult
{
    public Guid Id { get; set; }
    public required string CaseNumber { get; set; }
    public required string Location { get; set; }
    public required string Plaintiff { get; set; }
    public required string Defendant { get; set; }
    public required CourtCaseStatus Status { get; set; }
    public CourtCaseTypes Type { get; set; }
    public CourtCaseOutcomes Outcome { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public List<CourtCaseDate> CourtCaseDates { get; set; } = [];
    public List<Domain.Documents.Document> Documents { get; set; } = [];
    public List<Domain.Invoices.Invoice> Invoices { get; set; } = [];
    public List<Domain.Lawyers.Lawyer> Lawyers { get; set; } = [];
}
