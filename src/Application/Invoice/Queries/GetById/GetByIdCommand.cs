using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Queries.GetById;

public record GetByIdCommand(Guid Id) : IRequest<ErrorOr<InvoiceResult>>;
