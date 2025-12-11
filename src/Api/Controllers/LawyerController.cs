using System.ComponentModel.DataAnnotations;
using Application.Common.Models;
using Application.Lawyer.Commands.Add;
using Application.Lawyer.Commands.Delete;
using Application.Lawyer.Commands.Update;
using Application.Lawyer.Queries.Get;
using Application.Lawyer.Queries.GetById;
using Contracts.Lawyer.Requests;
using Contracts.Lawyer.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Handles operations related to lawyers.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LawyerController : ApiControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public LawyerController(
        ISender sender,
        IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets a status message for the LawyerController.
    /// </summary>
    /// <returns>Status message.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [EndpointName("GetAllLawyers")]
    public async Task<IActionResult> GetAll()
    {

        var command = new GetCommand();

        var result = await _sender.Send(command);

        return MatchAndMapOkResult<IEnumerable<LawyerResult>, IEnumerable<LawyerResponse>>(result, _mapper);
    }

    /// <summary>
    /// Gets a lawyer by its ID.
    /// </summary>
    /// <param name="id">The ID of the lawyer.</param>
    /// <returns>The lawyer with the specified ID.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("GetLawyersById")]
    public async Task<IActionResult> GetById([FromRoute][Required] string id)
    {

        var command = new GetByIdCommand(new Guid(id));

        var result = await _sender.Send(command);

        return MatchAndMapOkResult<LawyerResult, LawyerResponse>(result, _mapper);
    }

    /// <summary>
    /// Creates a new lawyer.
    /// </summary>
    /// <param name="lawyer">The lawyer object to create.</param>
    /// <returns>The created lawyer.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [EndpointName("CreateLawyers")]
    public async Task<IActionResult> Create([FromBody] AddLawyerRequest lawyer)
    {
        var command = _mapper.Map<AddCommand>(lawyer);

        var result = await _sender.Send(command);

        return MatchAndMapNoContentResult<Guid>(result, _mapper);
    }

    /// <summary>
    /// Updates an existing lawyer.
    /// </summary>
    /// <param name="id">The ID of the lawyer to update.</param>
    /// <param name="lawyer">The updated lawyer object.</param>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("UpdateLawyers")]
    public async Task<IActionResult> Update([FromRoute][Required] string id, [FromBody][Required] UpdateLawyerRequest lawyer)
    {
        var command = _mapper.Map<UpdateCommand>(lawyer);
        command.Id = new Guid(id);

        var result = await _sender.Send(command);

        return MatchAndMapNoContentResult<bool>(result, _mapper);                                        
    }

    /// <summary>
    /// Deletes a lawyer by its ID.
    /// </summary>
    /// <param name="id">The ID of the lawyer to delete.</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("DeleteLawyers")]
    public async Task<IActionResult> Delete([FromRoute][Required] string id)
    {
        var command = new DeleteCommand(new Guid(id));

        var result = await _sender.Send(command);

        return MatchAndMapNoContentResult<bool>(result, _mapper);
    }
}
