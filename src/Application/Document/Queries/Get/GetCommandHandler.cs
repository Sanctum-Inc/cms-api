using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Document.Queries.Get;
public class GetCommandHandler : IRequestHandler<GetCommand, ErrorOr<IEnumerable<DocumentResult>>>
{
    private readonly IDocumentService _documentService;
    public GetCommandHandler(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    public async Task<ErrorOr<IEnumerable<DocumentResult>>> Handle(GetCommand request, CancellationToken cancellationToken)
    {
        var result = await _documentService.Get(cancellationToken);

        return result;
    }
}
