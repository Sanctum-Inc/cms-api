namespace Application.Common.Models;
public record InvoiceItemResult(
        Guid Id,
        DateTime Date,
        string Description,
        int Hours,
        float CostPerHour,
        bool IsDayFee,
        decimal DayFeeAmount,
        decimal Total
    );

