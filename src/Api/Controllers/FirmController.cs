using Application.Common.Models;
using Application.Firm.Commands.Add;
using Application.Firm.Commands.Delete;
using Application.Firm.Commands.Update;
using Application.Firm.Queries.Get;
using Contracts.Firm.Requests;
using Contracts.Firm.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[ProducesErrorResponseType(typeof(ProblemDetails))]
public class FirmController : ApiControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISender _sender;

    public FirmController(IMapper mapper, ISender sender)
    {
        _sender = sender;
        _mapper = mapper;
    }

    // GET /api/Firm/{id}
    [HttpGet]
    [ProducesResponseType(typeof(FirmResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("GetFirmById")]
    public async Task<IActionResult> GetById()
    {
        var query = new GetCommand();

        var result = await _sender.Send(query);

        return MatchAndMapOkResult<FirmResult, FirmResponse>(result, _mapper);
    }

    // POST /api/Firm
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [EndpointName("CreateFirm")]
    public async Task<IActionResult> Create([FromBody] AddFirmRequest request)
    {
        var command = _mapper.Map<AddCommand>(request);

        var created = await _sender.Send(command);

        return MatchAndMapCreatedResult(created, _mapper);
    }

    // PUT /api/Firm/{id}
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("UpdateFirm")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateFirmRequest request)
    {
        var command = _mapper.Map<UpdateCommand>(request) with { Id = id };

        var updated = await _sender.Send(command);

        return MatchAndMapNoContentResult(updated, _mapper);
    }

    // DELETE /api/Firm/{id}
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("DeleteFirm")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteCommand(id);

        var deleted = await _sender.Send(command);

        return MatchAndMapNoContentResult(deleted, _mapper);
    }
}
