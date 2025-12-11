using Contracts.CourtCases.Responses;
using Contracts.InvoiceItem.Responses;

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
        bool IsPaid,
        string CaseNumber,
        string Plaintiff,
        string Defendant,
        Guid CaseId,
        IEnumerable<InvoiceItemResponse> Items
    );
