using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Document.Commands.Update;

public class UpdateCommandHandler : IRequestHandler<UpdateCommand, ErrorOr<bool>>
{
    private readonly IDocumentService _documentService;

    public UpdateCommandHandler(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    public async Task<ErrorOr<bool>> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var result = await _documentService.Update(request, cancellationToken);

        return result;
    }
}
