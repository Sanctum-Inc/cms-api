using ErrorOr;
using MediatR;

namespace Application.InvoiceItem.Commands.Update;

public record UpdateCommand(
    Guid Id,
    Guid InvoiceId,
    string Name,
    int Hours,
    decimal CostPerHour,
    Guid CaseId
) : IRequest<ErrorOr<bool>>;
