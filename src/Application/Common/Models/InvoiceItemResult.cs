namespace Application.Common.Models;

public record InvoiceItemResult(
    Guid Id,
    DateTime Date,
    string Name,
    int Hours,
    decimal CostPerHour,
    decimal Total
);
