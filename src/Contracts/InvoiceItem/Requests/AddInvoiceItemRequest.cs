namespace Contracts.InvoiceItem.Requests;
public record AddInvoiceItemRequest(
    Guid InvoiceId,
    string Name,
    int Hours,
    decimal? CostPerHour,
    decimal? DayFeeAmount,
    Guid CaseId,
    bool IsDayFee);
