using Application.Document.Queries.Get;
using ErrorOr;
using MediatR;

namespace Application.Document.Queries.GetById;
public record GetByIdCommand(Guid Id) : IRequest<ErrorOr<DocumentResult?>>;
