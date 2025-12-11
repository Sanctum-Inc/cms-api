using Application.Common.Models;
using Domain.Invoices;

namespace Application.Common.Models;
public record InvoiceResult(
    Guid Id,
    string InvoiceNumber,
    DateTime InvoiceDate,
    string ClientName,
    string? Reference,
    decimal TotalAmount,
    string AccountName,
    string Bank,
    string BranchCode,
    string AccountNumber,
    InvoiceStatus Status,
    string CaseNumber,
    string Plaintiff,
    string Defendant,
    Guid CaseId,
    IEnumerable<InvoiceItemResult> Items
);
