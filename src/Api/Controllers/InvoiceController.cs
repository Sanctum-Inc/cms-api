using Application.Common.Models;
using Application.Invoice.Commands.Add;
using Application.Invoice.Commands.CreatePdfLink;
using Application.Invoice.Commands.Delete;
using Application.Invoice.Commands.GeneratePdf;
using Application.Invoice.Commands.SetIsPaid;
using Application.Invoice.Commands.Update;
using Application.Invoice.Commands.ViewPDF;
using Application.Invoice.Queries.Get;
using Application.Invoice.Queries.GetById;
using Application.Invoice.Queries.GetInvoiceNumbers;
using Contracts.Invoice.Requests;
using Contracts.Invoice.Responses;
using Contracts.InvoiceItem.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
///     Handles operations related to invoices.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ApiControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISender _sender;

    public InvoiceController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    // GET /api/CourtCase
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<InvoiceResponse>), StatusCodes.Status200OK)]
    [EndpointName("GetAllInvoices")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _sender.Send(new GetCommand());

        return MatchAndMapOkResult<IEnumerable<InvoiceResult>, IEnumerable<InvoiceResponse>>(result, _mapper);
    }

    // GET /api/CourtCase/invoice-numbers
    [HttpGet("invoice-numbers")]
    [ProducesResponseType(typeof(IEnumerable<InvoiceNumberResponse>), StatusCodes.Status200OK)]
    [EndpointName("GetAllInvoiceNumbers")]
    public async Task<IActionResult> GetAllInvoiceNumbers()
    {
        var result = await _sender.Send(new GetInvoiceNumbersQuery());

        return MatchAndMapOkResult<IEnumerable<InvoiceNumbersResult>, IEnumerable<InvoiceNumberResponse>>(result,
            _mapper);
    }

    // GET /api/Invoice/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(InvoiceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("GetInvoicesById")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _sender.Send(new GetByIdCommand(id));

        return MatchAndMapOkResult<InvoiceResult, InvoiceResponse>(result, _mapper);
    }

    /// <summary>Generate PDF for invoice to Download</summary>
    /// <param name="id">Invoice Id</param>
    /// <returns>PDF file</returns>
    [HttpGet("pdf/download/{id}")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/pdf")]
    [EndpointName("GeneratePDF")]
    public async Task<IActionResult> GeneratePDF(Guid id)
    {
        var result = await _sender.Send(new GeneratePdfCommand(id));

        return result.Match<IActionResult>(
            data => File(data.Stream, data.ContentType, data.FileName),
            errors => Problem(
                string.Join(", ", errors.Select(e => e.Description)),
                    statusCode: StatusCodes.Status404NotFound) );
    }

    /// <summary>Generate PDF for invoice to View</summary>
    /// <param name="id">Invoice Id</param>
    /// <returns>PDF file</returns>
    [HttpGet("pdf/{id}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("CreateLink")]
    public async Task<IActionResult> CreateLink(Guid id)
    {
        var result = await _sender.Send(new CreatePdfLinkCommand(id));

        return MatchAndMapOkResult<string, string>(result, _mapper);
    }

    [AllowAnonymous]
    [HttpGet("pdf/view/{id}")]
    [Produces("application/pdf")]
    public async Task<IActionResult> ViewPdf(
        Guid id,
        [FromQuery] long expiry,
        [FromQuery] string signature,
        [FromQuery] Guid firmId)
    {
        var result = await _sender.Send(new ViewPdfCommand(id, expiry, signature, firmId));

        return result.Match<IActionResult>(
            pdf =>
            {
                Response.Headers.ContentDisposition =
                    $"inline; filename=\"{pdf.FileName}\"";

                return File(
                    pdf.Stream,
                    "application/pdf",
                    enableRangeProcessing: true
                );
            },
            errors => Problem(errors)
        );
    }

    // POST /api/Invoice
    [HttpPost]
    [ProducesResponseType(typeof(bool), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [EndpointName("CreateInvoices")]
    public async Task<IActionResult> Create([FromBody] AddInvoiceRequest request)
    {
        var command = _mapper.Map<AddCommand>(request);

        var created = await _sender.Send(command);

        return MatchAndMapCreatedResult(created, _mapper);
    }

    // PUT /api/Invoice/{id}
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("UpdateInvoices")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateInvoiceRequest request)
    {
        var command = _mapper.Map<UpdateCommand>(request) with { Id = id };

        var updated = await _sender.Send(command);

        return MatchAndMapNoContentResult(updated, _mapper);
    }

    // PUT /api/Invoice/{id}
    [HttpPut("status/{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("UpdateInvoicesStatus")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateInvoiceStatusRequest request)
    {
        var command = _mapper.Map<SetIsPaidCommand>(request) with { Id = id };

        var updated = await _sender.Send(command);

        return MatchAndMapNoContentResult(updated, _mapper);
    }

    // DELETE /api/Invoice/{id}
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("DeleteInvoices")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteCommand(id);

        var success = await _sender.Send(command);

        return MatchAndMapNoContentResult(success, _mapper);
    }
}
