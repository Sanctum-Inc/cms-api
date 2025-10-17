using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Document.Commands.Add;
public class AddCommandHandler : IRequestHandler<AddCommand, ErrorOr<bool>>
{
    private readonly IDocumentService _documentService;
    public AddCommandHandler(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    public async Task<ErrorOr<bool>> Handle(AddCommand request, CancellationToken cancellationToken)
    {
       var result = await _documentService.Add(request.File, request.Name, request.CaseId, cancellationToken);

        return result;
    }
}
