using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Domain.InvoiceItems;

namespace Domain.Invoices;
public class Invoice : AuditableEntity
{
    [Key]
    public Guid Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }

    public string ClientName { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public string CaseName { get; set; } = string.Empty;

    public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
    public decimal TotalAmount { get; set; }

    public string AccountName { get; set; } = string.Empty;
    public string Bank { get; set; } = string.Empty;
    public string BranchCode { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
}
