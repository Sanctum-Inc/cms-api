using Application.Common.Interfaces.Services;
using ErrorOr;
using FluentValidation;
using MediatR;

namespace Application.Invoice.Commands.SetIsPaid;
public class SetIsPaidCommandHandler : IRequestHandler<SetIsPaidCommand, ErrorOr<bool>>
{
    private readonly IInvoiceService _invoiceService;
    public SetIsPaidCommandHandler(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    public async Task<ErrorOr<bool>> Handle(SetIsPaidCommand request, CancellationToken cancellationToken)
    {
        var result = await _invoiceService.UpdateIsPaid(request, cancellationToken);

        return result;
    }
}
