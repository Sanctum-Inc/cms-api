using ErrorOr;
using MediatR;

namespace Application.InvoiceItem.Commands.Delete;
public record DeleteCommand(Guid Id) : IRequest<ErrorOr<bool>>;
