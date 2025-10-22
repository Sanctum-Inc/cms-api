namespace Contracts.InvoiceItem.Responses;
public record InvoiceItemResponse(
       Guid Id,
       DateTime Date,
       string Name,
       int Hours,
       decimal? CostPerHour,
       bool IsDayFee,
       decimal? DayFeeAmount,
       decimal Total
   );
