namespace Contracts.InvoiceItem.Requests;
public record UpdateInvoiceItemRequest(
    Guid InvoiceId,
    string Name,
    int Hours,
    decimal CostPerHour,
    Guid CaseId);
