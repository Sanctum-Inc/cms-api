using System.ComponentModel.DataAnnotations;
using Application.Common.Models;
using Application.CourtCase.Commands.Add;
using Application.CourtCase.Commands.Delete;
using Application.CourtCase.Commands.Update;
using Application.CourtCase.Queries.Get;
using Application.CourtCase.Queries.GetById;
using Contracts.Common;
using Contracts.CourtCases.Requests;
using Contracts.CourtCases.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Handles operations related to court cases.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CourtCaseController : ApiControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public CourtCaseController(ISender sender, IMapper mapper) : base(mapper, sender)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets all court cases.
    /// </summary>
    /// <returns>List of court cases.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var command = new GetCommand();

        var result = await _sender.Send(command);

        return MatchAndMapOkResult<IEnumerable<CourtCaseResult>, IEnumerable<CourtCasesResponse>>(result, _mapper);
    }

    /// <summary>
    /// Gets a court case by its ID.
    /// </summary>
    /// <param name="id">The ID of the court case.</param>
    /// <returns>The court case with the specified ID.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute][Required] string id)
    {
        var command = new GetByIdCommand(new Guid(id));

        var result = await _sender.Send(command);

        return MatchAndMapOkResult<CourtCaseResult, CourtCasesResponse>(result, _mapper);
    }

    /// <summary>
    /// Creates a new court case.
    /// </summary>
    /// <param name="addRequest">The court case object to create.</param>
    /// <returns>The created court case.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody][Required] AddCourtCaseRequest addRequest)
    {
        var command = _mapper.Map<AddCommand>(addRequest);

        var result = await _sender.Send(command);

        return MatchAndMapCreatedResult<bool>(result, _mapper);
    }

    /// <summary>
    /// Updates an existing court case.
    /// </summary>
    /// <param name="id">The ID of the court case to update.</param>
    /// <param name="updateRequest">The updated court case object.</param>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute][Required] string id,
        [FromBody][Required] UpdateCourtCaseRequest updateRequest)
    {
        var command = _mapper.Map<UpdateCommand>(updateRequest);
        // Override the Id from the route
        command = command with { Id = id };

        var result = await _sender.Send(command);

        return MatchAndMapNoContentResult<bool>(result, _mapper);
    }

    /// <summary>
    /// Deletes a court case by its ID.
    /// </summary>
    /// <param name="id">The ID of the court case to delete.</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute][Required] string id)
    {
        var command = new DeleteCommand(new Guid(id));

        var result = await _sender.Send(command);

        return MatchAndMapNoContentResult<bool>(result, _mapper);
    }
}
