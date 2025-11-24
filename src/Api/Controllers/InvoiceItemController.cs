using Application.Common.Models;
using Application.InvoiceItem.Commands.Add;
using Application.InvoiceItem.Commands.Delete;
using Application.InvoiceItem.Commands.Update;
using Application.InvoiceItem.Queries.Get;
using Application.InvoiceItem.Queries.GetById;
using Contracts.CourtCases.Responses;
using Contracts.InvoiceItem.Requests;
using Contracts.InvoiceItem.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class InvoiceItemController : ApiControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public InvoiceItemController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    // GET /api/CourtCase
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<InvoiceItemResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _sender.Send(new GetCommand());

        return MatchAndMapOkResult<IEnumerable<InvoiceItemResult>, IEnumerable<InvoiceItemResponse>>(result, _mapper);
    }

    // GET /api/CourtCase/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(InvoiceItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _sender.Send(new GetByIdCommand(id));

        return MatchAndMapOkResult<InvoiceItemResult, InvoiceItemResponse>(result, _mapper);
    }

    // POST /api/CourtCase
    [HttpPost]
    [ProducesResponseType(typeof(bool), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] AddInvoiceItemRequest request)
    {
        var command = _mapper.Map<AddCommand>(request);

        var created = await _sender.Send(command);

        return MatchAndMapCreatedResult<bool>(created, _mapper);
    }

    // PUT /api/CourtCase/{id}
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateInvoiceItemRequest request)
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
