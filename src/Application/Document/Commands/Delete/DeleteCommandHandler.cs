using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Document.Commands.Delete;
public class DeleteCommandHandler : IRequestHandler<DeleteCommand, ErrorOr<bool>>
{
    private readonly IDocumentService _documentService;
    public DeleteCommandHandler(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    public async Task<ErrorOr<bool>> Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        var result = await _documentService.Delete(request.Id, cancellationToken);

        return result;
    }
}
