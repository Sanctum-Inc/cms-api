namespace Contracts.InvoiceItem.Requests;
public record AddInvoiceItemRequest(
    string InvoiceId,
    string Name,
    int Hours,
    decimal CostPerHour,
    string Date,
    Guid CaseId,
    string? ClientName,
    string? Refference);
