using System.Linq;
using System.Threading;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Common.Models;
using Application.Invoice.Commands.Add;
using Application.Invoice.Commands.Update;
using Domain.InvoiceItems;
using Domain.Invoices;
using ErrorOr;
using Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;

namespace Infrastructure.Services;
public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ISessionResolver _sessionResolver;
    private readonly IFirmRepository _firmRepository;

    public InvoiceService(
        IInvoiceRepository invoiceRepository,
        ISessionResolver sessionResolver,
        IFirmRepository firmRepository)
    {
        _invoiceRepository = invoiceRepository;
        _sessionResolver = sessionResolver;
        _firmRepository = firmRepository;
    }

    public async Task<ErrorOr<bool>> Add(IRequest<ErrorOr<bool>> request, CancellationToken cancellationToken)
    {
        if (request is not AddCommand addCommand)
            return Error.Failure(description: "Invalid request type.");

        var userId = _sessionResolver.UserId;
        if (string.IsNullOrEmpty(userId))
            return Error.Unauthorized(description: "User is not authenticated.");

        var invoiceItem = new Invoice()
        {
            Id = Guid.NewGuid(),
            InvoiceNumber = addCommand.InvoiceNumber,
            InvoiceDate = addCommand.InvoiceDate,
            ClientName = addCommand.ClientName,
            Reference = addCommand.Reference,
            CaseName = addCommand.CaseName,
            AccountName = addCommand.AccountName,
            Bank = addCommand.Bank,
            BranchCode = addCommand.BranchCode,
            AccountNumber = addCommand.AccountNumber,
            TotalAmount = 0m
        };

        await _invoiceRepository.AddAsync(invoiceItem, cancellationToken);
        await _invoiceRepository.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<ErrorOr<bool>> Delete(Guid id, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id, cancellationToken);

        if (invoice == null)
            return Error.NotFound("Invoice.NotFound", "Invoice with given Id was not found.");


        await _invoiceRepository.DeleteAsync(invoice, cancellationToken);
        await _invoiceRepository.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<ErrorOr<DownloadDocumentResult>> GenerateInvoicePdf(Guid id, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id, cancellationToken);

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

    public async Task<ErrorOr<IEnumerable<InvoiceResult>>> Get(CancellationToken cancellationToken)
    {
        var invoices = await _invoiceRepository.GetAll(cancellationToken);

        var result = invoices.Select(invoice => new InvoiceResult(
            invoice.Id,
            invoice.InvoiceNumber,
            invoice.InvoiceDate,
            invoice.ClientName,
            invoice.Reference,
            invoice.CaseName,
            invoice.TotalAmount,
            invoice.AccountName,
            invoice.Bank,
            invoice.BranchCode,
            invoice.AccountNumber,
            (IReadOnlyList<InvoiceItemResult>)invoice.Items));

        return result.ToErrorOr();
    }

    public async Task<ErrorOr<InvoiceResult?>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id, cancellationToken);

        return invoice is null ?
            null :
            new InvoiceResult(
                invoice.Id,
                invoice.InvoiceNumber,
                invoice.InvoiceDate,
                invoice.ClientName,
                invoice.Reference,
                invoice.CaseName,
                invoice.TotalAmount,
                invoice.AccountName,
                invoice.Bank,
                invoice.BranchCode,
                invoice.AccountNumber,
                (IReadOnlyList<InvoiceItemResult>)invoice.Items);
    }

    public async Task<ErrorOr<bool>> Update(IRequest<ErrorOr<bool>> request, CancellationToken cancellationToken)
    {
        if (request is not UpdateCommand updateCommand)
            return Error.Failure(description: "Invalid request type.");

        var invoice = await _invoiceRepository.GetByIdAsync(updateCommand.Id, cancellationToken);
        if (invoice == null)
            return Error.NotFound("Invoice.NotFound", "Invoice with given Id was not found.");

        invoice.InvoiceNumber = updateCommand.InvoiceNumber;
        invoice.InvoiceDate = updateCommand.InvoiceDate;
        invoice.ClientName = updateCommand.ClientName;
        invoice.Reference = updateCommand.Reference;
        invoice.CaseName = updateCommand.CaseName;
        invoice.AccountName = updateCommand.AccountName;
        invoice.Bank = updateCommand.Bank;
        invoice.BranchCode = updateCommand.BranchCode;
        invoice.AccountNumber = updateCommand.AccountNumber;

        await _invoiceRepository.UpdateAsync(invoice, cancellationToken);
        await _invoiceRepository.SaveChangesAsync(cancellationToken);

        return true;

    }
}
