using Domain.CourtCaseDates;
using Domain.Documents;
using Domain.InvoiceItems;

namespace Contracts.CourtCases.Responses;
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
}
