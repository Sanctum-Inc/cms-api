using System.ComponentModel.DataAnnotations;
using Application.Common.Models;
using Application.Invoice.Commands.Add;
using Application.Invoice.Commands.Delete;
using Application.Invoice.Commands.Update;
using Application.Invoice.Queries.Get;
using Application.Invoice.Queries.GetById;
using Contracts.CourtCases.Responses;
using Contracts.Invoice.Requests;
using Contracts.Invoice.Responses;
using Contracts.InvoiceItem.Requests;
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
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public InvoiceController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    // GET /api/CourtCase
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<InvoiceResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _sender.Send(new GetCommand());

        return MatchAndMapOkResult<IEnumerable<InvoiceResult>, IEnumerable<InvoiceResponse>>(result, _mapper);
    }

    // GET /api/CourtCase/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(InvoiceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _sender.Send(new GetByIdCommand(id));

        return MatchAndMapOkResult<InvoiceResult, InvoiceResponse>(result, _mapper);

    }

    // POST /api/CourtCase
    [HttpPost]
    [ProducesResponseType(typeof(bool), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] AddInvoiceRequest request)
    {
        var command = _mapper.Map<AddCommand>(request);

        var created = await _sender.Send(command);

        return MatchAndMapCreatedResult<bool>(created, _mapper);
    }

    // PUT /api/CourtCase/{id}
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateInvoiceRequest request)
    {
        var command = _mapper.Map<UpdateCommand>(request) with { Id = id };

        var updated = await _sender.Send(command);

        return MatchAndMapNoContentResult<bool>(updated, _mapper);
    }

    // DELETE /api/CourtCase/{id}
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteCommand(id);

        var success = await _sender.Send(command);

        return MatchAndMapNoContentResult<bool>(success, _mapper);
    }
}
