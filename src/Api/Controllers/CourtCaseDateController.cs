
using Application.Common.Models;
using Application.CourtCaseDates.Commands.Add;
using Application.CourtCaseDates.Commands.Delete;
using Application.CourtCaseDates.Commands.Update;
using Application.CourtCaseDates.Queries.Get;
using Application.CourtCaseDates.Queries.GetById;
using Contracts.CourtCaseDates.Requests;
using Contracts.CourtCaseDates.Responses;
using Contracts.CourtCases.Responses;
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
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public CourtCaseDateController(IMapper mapper, ISender sender)
    {
        _sender = sender;
        _mapper = mapper;
    }

    // GET /api/CourtCase
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CourtCaseDatesResponse>), StatusCodes.Status200OK)]
    [EndpointName("GetAllCourtCaseDates")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _sender.Send(new GetCommand());

        return MatchAndMapOkResult<IEnumerable<CourtCaseDateResult>, IEnumerable<CourtCaseDatesResponse>>(result, _mapper);
    }

    // GET /api/CourtCase/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CourtCaseDatesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("GetCourtCaseDatesById")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _sender.Send(new GetByIdCommand(id));

        return MatchAndMapOkResult<CourtCaseDateResult, CourtCaseDatesResponse>(result, _mapper);
    }

    // POST /api/CourtCase
    [HttpPost]
    [ProducesResponseType(typeof(bool), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [EndpointName("CreateCourtCaseDates")]
    public async Task<IActionResult> Create([FromBody] AddCourtCaseDateRequest request)
    {
        var command = _mapper.Map<AddCommand>(request);

        var created = await _sender.Send(command);

        return MatchAndMapCreatedResult<Guid>(created, _mapper);
    }

    // PUT /api/CourtCase/{id}
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("UpdateCourtCaseDates")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCourtCaseDateRequest request)
    {
        var command = _mapper.Map<UpdateCommand>(request) with { Id = id };

        var updated = await _sender.Send(command);

        return MatchAndMapNoContentResult<bool>(updated, _mapper);
    }

    // DELETE /api/CourtCase/{id}
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("DeleteCourtCaseDates")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteCommand(id);

        var success = await _sender.Send(command);

        return MatchAndMapNoContentResult<bool>(success, _mapper);
    }
}
