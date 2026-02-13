using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Commands.CreatePdfLink;

public class CreatePdfLinkCommandHandler : IRequestHandler<CreatePdfLinkCommand, ErrorOr<string>>
{
    private readonly IInvoiceService _invoiceService;

    public CreatePdfLinkCommandHandler(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    public async Task<ErrorOr<string>> Handle(CreatePdfLinkCommand request, CancellationToken cancellationToken)
    {
        var result = await _invoiceService.CreatePdfLink(request.Id, cancellationToken);

        return result;
    }
}
