using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Commands.Delete;
public class DeleteCommandHandler : IRequestHandler<DeleteCommand, ErrorOr<bool>>
{
    private readonly IInvoiceService _invoiceService;
    public DeleteCommandHandler(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    public async Task<ErrorOr<bool>> Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        var result = await _invoiceService.Delete(request.Id, cancellationToken);

        return result;
    }
}
