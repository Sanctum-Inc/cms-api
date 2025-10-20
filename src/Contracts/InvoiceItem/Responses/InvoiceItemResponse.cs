namespace Contracts.InvoiceItem.Responses;
public record InvoiceItemResponse(
       Guid Id,
       DateTime Date,
       string Description,
       int Hours,
       float CostPerHour,
       bool IsDayFee,
       decimal DayFeeAmount,
       decimal Total
   );
