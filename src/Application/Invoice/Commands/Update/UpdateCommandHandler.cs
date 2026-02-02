using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Commands.Update;

public class UpdateCommandHandler : IRequestHandler<UpdateCommand, ErrorOr<bool>>
{
    private readonly IInvoiceService _invoiceService;

    public UpdateCommandHandler(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    public async Task<ErrorOr<bool>> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var result = await _invoiceService.Update(request, cancellationToken);

        return result;
    }
}
