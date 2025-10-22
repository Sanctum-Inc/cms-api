using System.ComponentModel.DataAnnotations;
using Application.Common.Models;
using Application.Invoice.Commands.Add;
using Application.Invoice.Commands.Delete;
using Application.Invoice.Commands.Update;
using Application.Invoice.Queries.Get;
using Application.Invoice.Queries.GetById;
using Contracts.Invoice.Requests;
using Contracts.Invoice.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Handles operations related to invoices.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ApiControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISender _sender;

    public InvoiceController(IMapper mapper, ISender sender)
    {
        _mapper = mapper;
        _sender = sender;
    }

    /// <summary>
    /// Gets a status message for the InvoiceController.
    /// </summary>
    /// <returns>Status message.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync()
    {

        var command = new GetCommand();

        var result = await _sender.Send(command);

        return MatchAndMapOkResult<IEnumerable<InvoiceResult>, IEnumerable<InvoiceResponse>>(result, _mapper);
    }

    /// <summary>
    /// Gets an invoice by its ID.
    /// </summary>
    /// <param name="id">The ID of the invoice.</param>
    /// <returns>The invoice with the specified ID.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute][Required] string id)
    {

        var command = new GetByIdCommand(new Guid(id));

        var result = await _sender.Send(command);

        return MatchAndMapOkResult<InvoiceResult, InvoiceResponse>(result, _mapper);
    }

    /// <summary>
    /// Creates a new invoice.
    /// </summary>
    /// <param name="invoice">The invoice object to create.</param>
    /// <returns>The created invoice.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody][Required] AddInvoiceRequest invoice)
    {
        var command = _mapper.Map<AddCommand>(invoice);

        var result = await _sender.Send(command);

        return MatchAndMapCreatedResult<bool>(result, _mapper);
    }

    /// <summary>
    /// Updates an existing invoice.
    /// </summary>
    /// <param name="id">The ID of the invoice to update.</param>
    /// <param name="invoice">The updated invoice object.</param>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync([FromRoute][Required] string id, [FromBody][Required] object invoice)
    {
        var command = _mapper.Map<UpdateCommand>(invoice);
        command = command with { Id = new Guid(id) };

        var result = await _sender.Send(command);

        return MatchAndMapNoContentResult<bool>(result, _mapper);
    }

    /// <summary>
    /// Deletes an invoice by its ID.
    /// </summary>
    /// <param name="id">The ID of the invoice to delete.</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute][Required] string id)
    {
        var command = new DeleteCommand(new Guid(id));

        var result = await _sender.Send(command);

        return MatchAndMapNoContentResult<bool>(result, _mapper);
    }
}
