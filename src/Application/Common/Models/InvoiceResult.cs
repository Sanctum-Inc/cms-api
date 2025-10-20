namespace Application.Common.Models;
public record InvoiceResult(
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
        string AccountNumber,
        IReadOnlyList<InvoiceItemResult> Items
    );
