namespace Contracts.InvoiceItem.Requests;
public record AddInvoiceItemRequest(
    Guid InvoiceId,
    string Name,
    int Hours,
    decimal CostPerHour,
    Guid CaseId);
