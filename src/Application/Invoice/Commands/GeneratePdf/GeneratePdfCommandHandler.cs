using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Commands.GeneratePdf;
public class GeneratePdfCommandHandler : IRequestHandler<GeneratePdfCommand, ErrorOr<DownloadDocumentResult>>
{
    public Task<ErrorOr<DownloadDocumentResult>> Handle(GeneratePdfCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
