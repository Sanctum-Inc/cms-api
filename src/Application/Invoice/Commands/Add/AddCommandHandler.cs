using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Commands.Add;

public class AddCommandHandler : IRequestHandler<AddCommand, ErrorOr<Guid>>
{
    private readonly IInvoiceService _invoiceService;

    public AddCommandHandler(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    public Task<ErrorOr<Guid>> Handle(AddCommand request, CancellationToken cancellationToken)
    {
        var result = _invoiceService.Add(request, cancellationToken);

        return result;
    }
}
