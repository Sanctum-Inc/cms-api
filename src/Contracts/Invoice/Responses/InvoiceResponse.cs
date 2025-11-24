using Contracts.InvoiceItem.Responses;

namespace Contracts.Invoice.Responses;
public record InvoiceResponse(
        Guid Id,
        string InvoiceNumber,
        DateTime InvoiceDate,
        string ClientName,
        string Reference,
        string CaseName,
        decimal TotalAmount,
        string AccountName,
        string Bank,
        string BranchCode,
        string AccountNumber
    );
