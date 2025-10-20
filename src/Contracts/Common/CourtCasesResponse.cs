using Domain.CourtCaseDates;
using Domain.Documents;
using Domain.InvoiceItems;

namespace Contracts.Common;
public class CourtCasesResponse
{
    public Guid Id { get; set; }
    public required string CaseNumber { get; set; }
    public required string Location { get; set; }
    public required string Plaintiff { get; set; }
    public required string Defendant { get; set; }
    public required string Status { get; set; }
    public string? Type { get; set; }
    public string? Outcome { get; set; }
    public DateTime DateCreated { get; set; }
    public Guid UserId { get; set; }
    public required Domain.Users.User User { get; set; }
    public List<CourtCaseDate> CourtCaseDates { get; set; } = [];
    public List<Document> Documents { get; set; } = [];
    public List<Domain.Invoices.Invoice> InvoiceItems { get; set; } = [];
    public List<Domain.Lawyers.Lawyer> Lawyers { get; set; } = [];
}
