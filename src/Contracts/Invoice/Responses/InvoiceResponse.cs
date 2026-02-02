using Contracts.InvoiceItem.Responses;
using Domain.Invoices;

namespace Contracts.Invoice.Responses;

public record InvoiceResponse(
    Guid Id,
    string InvoiceNumber,
    DateTime InvoiceDate,
    string ClientName,
    string Reference,
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
    IEnumerable<InvoiceItemResponse> Items
);
