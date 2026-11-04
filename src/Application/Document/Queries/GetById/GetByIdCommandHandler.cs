using Application.Common.Interfaces.Services;
using Application.Document.Queries.Get;
using ErrorOr;
using MediatR;

namespace Application.Document.Queries.GetById;

public class GetByIdCommandHandler(IDocumentService documentService)
    : IRequestHandler<GetByIdCommand, ErrorOr<DocumentResult>>
{
    public async Task<ErrorOr<DocumentResult>> Handle(GetByIdCommand request, CancellationToken cancellationToken)
    {
        var result = await documentService.GetById(request.Id, cancellationToken);

        return result;
    }
}
