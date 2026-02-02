using Domain.Invoices;

namespace Contracts.Invoice.Requests;

public record UpdateInvoiceRequest(
    Guid Id,
    string InvoiceNumber,
    DateTime InvoiceDate,
    string ClientName,
    string Reference,
    string CaseName,
    string AccountName,
    string Bank,
    string BranchCode,
    string AccountNumber,
    InvoiceStatus Status
);
