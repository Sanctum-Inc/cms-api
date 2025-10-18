using Domain.CourtCaseDates;
using Domain.InvoiceItems;
using Domain.Users;

namespace Application.Common.Models;
public class CourtCaseResult
{
    public Guid Id { get; set; }
    public required string CaseNumber { get; set; }
    public required string Location { get; set; }
    public required string Plaintiff { get; set; }
    public required string Defendant { get; set; }
    public required string Status { get; set; }
    public string? Type { get; set; }
    public string? Outcome { get; set; }
    public DateTime Created { get; set; }
    public Guid UserId { get; set; }
    public required User User { get; set; }
    public List<CourtCaseDate> CourtCaseDates { get; set; } = [];
    public List<Domain.Documents.Document> Documents { get; set; } = [];
    public List<InvoiceItem> InvoiceItems { get; set; } = [];
    public List<Domain.Lawyers.Lawyer> Lawyers { get; set; } = [];
}
