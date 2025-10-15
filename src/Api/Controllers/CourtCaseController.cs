using Application.CourtCase.Commands.Add;
using Application.CourtCase.Queries.Get;
using Contracts.CourtCases.Requests;
using Contracts.CourtCases.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Api.Controllers;

/// <summary>
/// Handles operations related to court cases.
/// </summary>
[ApiController]
[Route("[controller]")]
[Authorize]
public class CourtCaseController : ApiControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public CourtCaseController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets a status message for the CourtCaseController.
    /// </summary>
    /// <returns>Status message.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var command = new GetCommand();

        var result = await _sender.Send(command);

        return MatchAndMapResult<GetCourtCaseResult, GetCourtCasesResponse>(result, _mapper);

    }

    /// <summary>
    /// Gets a court case by its ID.
    /// </summary>
    /// <param name="id">The ID of the court case.</param>
    /// <returns>The court case with the specified ID.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById([FromRoute][Required] int id)
    {
        return Ok($"CourtCaseController received ID: {id}");
    }

    /// <summary>
    /// Creates a new court case.
    /// </summary>
    /// <param name="courtCase">The court case object to create.</param>
    /// <returns>The created court case.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody][Required] AddCourtCaseRequest addRequest)
    {
        var command = _mapper.Map<AddCommand>(addRequest);

        var result = await _sender.Send(command);

        return MatchAndMapResult<Guid, Guid>(result, _mapper);
    }

    /// <summary>
    /// Updates an existing court case.
    /// </summary>
    /// <param name="id">The ID of the court case to update.</param>
    /// <param name="courtCase">The updated court case object.</param>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update([FromRoute][Required] int id, [FromBody][Required] object courtCase)
    {
        return NoContent(); // Assuming update is successful
    }

    /// <summary>
    /// Deletes a court case by its ID.
    /// </summary>
    /// <param name="id">The ID of the court case to delete.</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete([FromRoute][Required] int id)
    {
        return NoContent(); // Assuming delete is successful
    }
}
