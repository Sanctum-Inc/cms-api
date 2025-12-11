using Domain.Invoices;

namespace Contracts.Invoice.Requests;
public record AddInvoiceRequest(
        string InvoiceNumber,
        DateTime InvoiceDate,
        string ClientName,
        string Reference,
        string CaseName,
        string AccountName,
        string Bank,
        string BranchCode,
        string AccountNumber,
        InvoiceStatus Status);
