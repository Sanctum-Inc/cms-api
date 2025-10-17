using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Document.Queries.Download;
public class DownloadCommandHandler : IRequestHandler<DownloadCommand, ErrorOr<DownloadDocumentResult?>>
{
    private readonly IDocumentService _documentService;
    public DownloadCommandHandler(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    public async Task<ErrorOr<DownloadDocumentResult?>> Handle(DownloadCommand request, CancellationToken cancellationToken)
    {
        var result = await _documentService.Download(request.Id, cancellationToken);

        return result!;
    }
}
