using Domain.Common;
using Domain.CourtCases;
using Domain.InvoiceItems;
using Domain.Users;

namespace Domain.Invoices;

public class Invoice : AuditableEntity
{
    public Guid Id { get; set; }

    public required string InvoiceNumber { get; set; }
    public DateTime InvoiceDate { get; set; }

    public required string ClientName { get; set; }
    public string? Reference { get; set; }
    public string? CaseName { get; set; }

    public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();

    public decimal TotalAmount { get; set; }

    public required string AccountName { get; set; }
    public required string Bank { get; set; }
    public required string BranchCode { get; set; }
    public required string AccountNumber { get; set; }

    public required Guid UserId { get; set; }
    public User? User { get; set; }

    public required Guid CaseId { get; set; }
    public CourtCase? Case { get; set; }
}
