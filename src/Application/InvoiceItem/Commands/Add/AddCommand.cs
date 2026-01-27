using ErrorOr;
using MediatR;

namespace Application.InvoiceItem.Commands.Add;
public record AddCommand(
    string InvoiceId,
    string Name,
    int Hours,
    decimal CostPerHour,
    Guid CaseId) : IRequest<ErrorOr<Guid>>;
