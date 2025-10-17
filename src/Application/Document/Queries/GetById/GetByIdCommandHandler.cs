using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Document.Queries.GetById;
public class GetByIdCommandHandler : IRequestHandler<GetByIdCommand, ErrorOr<GetDocumentByIdResult?>>
{
    private readonly IDocumentService _documentService;
    public GetByIdCommandHandler(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    public async Task<ErrorOr<GetDocumentByIdResult?>> Handle(GetByIdCommand request, CancellationToken cancellationToken)
    {
        var result = await _documentService.GetById(request.Id, cancellationToken);

        return result;
    }
}
