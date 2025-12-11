using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Document.Commands.Add;
public class AddCommandHandler : IRequestHandler<AddCommand, ErrorOr<Guid>>
{
    private readonly IDocumentService _documentService;
    public AddCommandHandler(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    public async Task<ErrorOr<Guid>> Handle(AddCommand request, CancellationToken cancellationToken)
    {
        var result = await _documentService.Add(request, cancellationToken);

        return result;
    }
}
