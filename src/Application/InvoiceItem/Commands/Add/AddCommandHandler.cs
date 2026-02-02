using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Domain.Invoices;
using ErrorOr;
using MediatR;

namespace Application.InvoiceItem.Commands.Add;

public class AddCommandHandler : IRequestHandler<AddCommand, ErrorOr<Guid>>
{
    private readonly IFirmService _firmService;
    private readonly IInvoiceItemService _invoiceItemService;
    private readonly IInvoiceService _invoiceService;
    private readonly ISessionResolver _sessionResolver;

    public AddCommandHandler(
        IInvoiceItemService invoiceItemService,
        IFirmService firmService,
        ISessionResolver sessionResolver,
        IInvoiceService invoiceService)
    {
        _invoiceItemService = invoiceItemService;
        _firmService = firmService;
        _sessionResolver = sessionResolver;
        _invoiceService = invoiceService;
    }

    public async Task<ErrorOr<Guid>> Handle(AddCommand request, CancellationToken cancellationToken)
    {
        List<Error>? invoiceErrors = [];
        if (request.InvoiceId.Equals("new", StringComparison.CurrentCultureIgnoreCase))
        {
            invoiceErrors = await AddNewInvoice(request, cancellationToken);
        }

        if (invoiceErrors?.Count > 0)
        {
            return invoiceErrors;
        }

        var result = await _invoiceItemService.Add(request, cancellationToken);

        return result;
    }

    private async Task<List<Error>?> AddNewInvoice(AddCommand request, CancellationToken cancellationToken)
    {
        var firm = await _firmService.GetById(new Guid(_sessionResolver.FirmId ?? ""), cancellationToken);

        if (firm.IsError)
        {
            return [Error.Conflict("No firm configuration found")];
        }

        var invoiceNumber = await _invoiceService.GetNewInvoiceNumber(cancellationToken);

        var number = int.Parse(invoiceNumber.Split('-')[1]);
        var newInvoiceNumber = $"INV-{number + 1:000}";

        var command = new Invoice.Commands.Add.AddCommand(
            newInvoiceNumber,
            DateTime.UtcNow,
            request.ClientName,
            request.Reference,
            firm.Value.AccountName,
            firm.Value.Bank,
            firm.Value.BranchCode,
            firm.Value.AccountNumber,
            request.CaseId,
            InvoiceStatus.DRAFT);

        var invoiceResult = await _invoiceService.Add(command, cancellationToken);

        if (invoiceResult.IsError)
        {
            return invoiceResult.Errors;
        }

        request.InvoiceId = invoiceResult.Value.ToString();

        return null;
    }
}
