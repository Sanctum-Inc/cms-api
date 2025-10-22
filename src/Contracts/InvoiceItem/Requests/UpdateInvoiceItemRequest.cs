namespace Contracts.InvoiceItem.Requests;
public record UpdateInvoiceItemRequest(
    Guid Id,
    Guid InvoiceId,
    string Name,
    int Hours,
    decimal? CostPerHour,
    decimal? DayFeeAmount,
    Guid CaseId,
    bool IsDayFee);
