using System.Linq;
using System.Threading;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Common.Models;
using Application.Invoice.Commands.Add;
using Application.Invoice.Commands.SetIsPaid;
using Application.Invoice.Commands.Update;
using Domain.InvoiceItems;
using Domain.Invoices;
using ErrorOr;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services.Base;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;

namespace Infrastructure.Services;
public class InvoiceService : BaseService<Invoice, InvoiceResult, AddCommand, UpdateCommand>, IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IFirmRepository _firmRepository;

    public InvoiceService(
        IInvoiceRepository invoiceRepository,
        IMapper mapper,
        IFirmRepository firmRepository,
        ISessionResolver sessionResolver) : base(invoiceRepository, mapper, sessionResolver)
    {
        _invoiceRepository = invoiceRepository;
        _firmRepository = firmRepository;
    }

    public async Task<ErrorOr<DownloadDocumentResult>> GenerateInvoicePdf(Guid id, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetByIdAndUserIdAsync(id, cancellationToken);

        if (invoice == null)
            return Error.NotFound("Invoice.NotFound", "Invoice with given Id was not found.");

        var firm = await _firmRepository.GetLatest(cancellationToken);

        if (firm == null)
            return Error.NotFound("Firm.NotFound", "Firm details not found.");

        var document = new PdfService(invoice, firm);
        var pdfBytes = document.GeneratePdf();

        // COnvert bytes to stream
        var stream = new MemoryStream(pdfBytes);
        stream.Position = 0; // reset to beginning

        return new DownloadDocumentResult(
            FileName: $"Invoice_{invoice.InvoiceNumber}.pdf",
            ContentType: "application/pdf",
            Stream: stream
        );
    }

    public override async Task<ErrorOr<IEnumerable<InvoiceResult>>> Get(CancellationToken cancellationToken)
    {
        var result = await _invoiceRepository.GetAll(cancellationToken);

        return result.Select(x => new InvoiceResult(
                x.Id,
                x.InvoiceNumber,
                x.InvoiceDate,
                x.ClientName,
                x.Reference,
                x.Items.Sum(x => x.CostPerHour * x.Hours),
                x.AccountName,
                x.Bank,
                x.BranchCode,
                x.AccountNumber,
                x.Status,
                x.Case!.CaseNumber,
                x.Case!.Plaintiff,
                x.Case!.Defendant,
                x.CaseId,
                x.Items.Select(x =>
                {
                    return new InvoiceItemResult(
                        x.Id,
                        x.Created,
                        x.Name,
                        x.Hours,
                        x.CostPerHour,
                        x.Hours * x.CostPerHour
                    );
                }))
        )
        .ToErrorOr();
    }

    public async Task<ErrorOr<IEnumerable<InvoiceNumbersResult>>> GetInvoiceNumbers(CancellationToken cancellationToken)
    {
        var result = await base.Get(cancellationToken);
        if (result.IsError)
        {
            return result.Errors;
        }

        return result.Value
            .Select(x => new InvoiceNumbersResult(x.Id.ToString(), x.InvoiceNumber))
            .ToErrorOr();
    }

    public async Task<string> GetNewInvoiceNumber(CancellationToken cancellationToken)
    {
        var result = await _invoiceRepository.GetNewInvoiceNumber(cancellationToken);

        return result;
    }

    public async Task<ErrorOr<bool>> UpdateIsPaid(SetIsPaidCommand request, CancellationToken cancellationToken)
    {
        var entity = await _invoiceRepository.GetByIdAndUserIdAsync( request.Id, cancellationToken );

        if ( entity == null )
            return Error.NotFound("Entity.NotFound", "Entity with given Id was not found.");

        entity.Status = request.isPaid;

        await _invoiceRepository.UpdateAsync(entity, cancellationToken);
        await _invoiceRepository.SaveChangesAsync(cancellationToken);

        return true;
    }

    protected override Guid GetIdFromUpdateCommand(UpdateCommand command)
    {
        return command.Id;
    }

    protected override ErrorOr<Invoice> MapFromAddCommand(AddCommand command, string? userId = null)
    {
        if (string.IsNullOrEmpty(userId))
            return Error.Unauthorized(description: "User is not authenticated.");

        return new Invoice()
        {
            Id = Guid.NewGuid(),
            InvoiceNumber = command.InvoiceNumber,
            InvoiceDate = command.InvoiceDate,
            UserId = Guid.Parse(userId),
            ClientName = command.ClientName,
            Reference = command.Reference,
            AccountName = command.AccountName,
            AccountNumber = command.AccountNumber,
            Bank = command.Bank,
            BranchCode = command.BranchCode,
            IsDeleted = false,
            CaseId = command.CaseId,
        };
    }

    protected override void MapFromUpdateCommand(Invoice entity, UpdateCommand command)
    {
        entity.InvoiceNumber = command.InvoiceNumber;
        entity.InvoiceDate = command.InvoiceDate;
        entity.ClientName = command.ClientName;
        entity.Reference = command.Reference;
        entity.AccountName = command.AccountName;
        entity.AccountNumber = command.AccountNumber;
        entity.Bank = command.Bank;
        entity.BranchCode = command.BranchCode;
    }
}
