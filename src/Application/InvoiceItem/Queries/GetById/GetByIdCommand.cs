using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.InvoiceItem.Queries.GetById;
public record GetByIdCommand(Guid Id) : IRequest<ErrorOr<InvoiceItemResult?>>;
