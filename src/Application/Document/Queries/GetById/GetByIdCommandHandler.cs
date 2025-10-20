using Application.Common.Interfaces.Services;
using Application.Document.Queries.Get;
using ErrorOr;
using MediatR;

namespace Application.Document.Queries.GetById;
public class GetByIdCommandHandler : IRequestHandler<GetByIdCommand, ErrorOr<DocumentResult?>>
{
    private readonly IDocumentService _documentService;
    public GetByIdCommandHandler(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    public async Task<ErrorOr<DocumentResult?>> Handle(GetByIdCommand request, CancellationToken cancellationToken)
    {
        var result = await _documentService.GetById(request.Id, cancellationToken);

        return result;
    }
}
