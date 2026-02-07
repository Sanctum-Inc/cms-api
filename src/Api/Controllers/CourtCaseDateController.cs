using Application.Common.Models;
using Application.CourtCaseDates.Commands.Add;
using Application.CourtCaseDates.Commands.Delete;
using Application.CourtCaseDates.Commands.SetToCancelled;
using Application.CourtCaseDates.Commands.SetToComplete;
using Application.CourtCaseDates.Commands.Update;
using Application.CourtCaseDates.Queries.Get;
using Application.CourtCaseDates.Queries.GetById;
using Contracts.CourtCaseDates.Requests;
using Contracts.CourtCaseDates.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CourtCaseDateController : ApiControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISender _sender;

    public CourtCaseDateController(IMapper mapper, ISender sender)
    {
        _sender = sender;
        _mapper = mapper;
    }

    // GET /api/CourtCaseDate
    [HttpGet]
    [ProducesResponseType(typeof(CourtCaseDateResponse), StatusCodes.Status200OK)]
    [EndpointName("GetAllCourtCaseDates")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _sender.Send(new GetCommand());

        return MatchAndMapOkResult<CourtCaseDateResult, CourtCaseDateResponse>(result,
            _mapper)!;
    }

    // GET /api/CourtCaseDate/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CourtCaseDateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("GetCourtCaseDatesById")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await _sender.Send(new GetByIdCommand(id));

        return MatchAndMapOkResult<CourtCaseDateResult, CourtCaseDateResponse>(result, _mapper)!;
    }

    // POST /api/CourtCaseDate
    [HttpPost]
    [ProducesResponseType(typeof(bool), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [EndpointName("CreateCourtCaseDates")]
    public async Task<IActionResult> Create([FromBody] AddCourtCaseDateRequest request)
    {
        var command = _mapper.Map<AddCommand>(request);

        var created = await _sender.Send(command);

        return MatchAndMapCreatedResult(created, _mapper);
    }

    // PUT /api/CourtCaseDate/{id}
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("UpdateCourtCaseDates")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCourtCaseDateRequest request)
    {
        var command = _mapper.Map<UpdateCommand>(request) with { Id = id };

        var updated = await _sender.Send(command);

        return MatchAndMapNoContentResult(updated, _mapper);
    }

    [HttpPatch("cancelled/{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("SetToCancelled")]
    public async Task<IActionResult> SetToCancelled([FromRoute] Guid id)
    {
        var command = new SetToCancelledCommand(id);

        var cancelled = await _sender.Send(command);

        return MatchAndMapNoContentResult(cancelled, _mapper);
    }

    [HttpPatch("complete/{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("SetToComplete")]
    public async Task<IActionResult> SetToComplete([FromRoute] Guid id)
    {
        var command = new SetToCompleteCommand(id);

        var completed = await _sender.Send(command);

        return MatchAndMapNoContentResult(completed, _mapper);
    }


    // DELETE /api/CourtCaseDate/{id}
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("DeleteCourtCaseDates")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var command = new DeleteCommand(id);

        var success = await _sender.Send(command);

        return MatchAndMapNoContentResult(success, _mapper);
    }
}
