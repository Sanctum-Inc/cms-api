using ErrorOr;
using MediatR;

namespace Application.InvoiceItem.Commands.Add;
public record AddCommand(
    Guid InvoiceId,
    string Name,
    int Hours,
    decimal CostPerHour) : IRequest<ErrorOr<Guid>>;
