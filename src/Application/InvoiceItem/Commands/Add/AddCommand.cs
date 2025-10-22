using ErrorOr;
using MediatR;

namespace Application.InvoiceItem.Commands.Add;
public record AddCommand(
    Guid InvoiceId,
    string Name,
    int Hours,
    decimal? CostPerHour,
    decimal? DayFeeAmount,
    bool IsDayFee) : IRequest<ErrorOr<bool>>;
