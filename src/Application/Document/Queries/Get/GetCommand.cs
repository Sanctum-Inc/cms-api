using ErrorOr;
using MediatR;

namespace Application.Document.Queries.Get;
public record GetCommand() : IRequest<ErrorOr<IEnumerable<GetDocumentResult?>>>;