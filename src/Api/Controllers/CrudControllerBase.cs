using System.ComponentModel.DataAnnotations;
using Application.Common.Models;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Provides a generic CRUD controller base implementation using MediatR and Mapster.
/// </summary>
/// <typeparam name="TResult">The domain or query result type.</typeparam>
/// <typeparam name="TResponse">The response DTO type.</typeparam>
/// <typeparam name="TGetCommand">The command for getting all records.</typeparam>
/// <typeparam name="TGetByIdCommand">The command for getting a single record by ID.</typeparam>
/// <typeparam name="TAddRequest">The request type for creating a new entity.</typeparam>
/// <typeparam name="TAddCommand">The command for creating a new entity.</typeparam>
/// <typeparam name="TUpdateRequest">The request type for updating an entity.</typeparam>
/// <typeparam name="TUpdateCommand">The command for updating an entity.</typeparam>
/// <typeparam name="TDeleteCommand">The command for deleting an entity.</typeparam>
[ApiController]
[Produces("application/json")]
public abstract class CrudControllerBase<
    TResult,
    TResponse,
    TGetCommand,
    TGetByIdCommand,
    TAddRequest,
    TAddCommand,
    TUpdateRequest,
    TUpdateCommand,
    TDeleteCommand
> : ApiControllerBase
    where TGetCommand : IRequest<ErrorOr<IEnumerable<TResult>>>, new()
    where TGetByIdCommand : IRequest<ErrorOr<TResult?>>
    where TAddCommand : IRequest<ErrorOr<bool>>
    where TUpdateCommand : IRequest<ErrorOr<bool>>
    where TDeleteCommand : IRequest<ErrorOr<bool>>
{
    protected readonly ISender Sender;
    protected readonly IMapper Mapper;

    protected CrudControllerBase(IMapper mapper, ISender sender)
    {
        Sender = sender;
        Mapper = mapper;
    }

    /// <summary>
    /// Gets all entities.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public virtual async Task<IActionResult> Get()
    {
        var command = new TGetCommand();
        var result = await Sender.Send(command);
        return MatchAndMapOkResult<IEnumerable<TResult>, IEnumerable<TResponse>>(result, Mapper);
    }

    /// <summary>
    /// Gets a single entity by its ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<IActionResult> GetById([FromRoute][Required] string id)
    {
        var command = (TGetByIdCommand)Activator.CreateInstance(typeof(TGetByIdCommand), new Guid(id))!;
        var result = await Sender.Send(command);
        return MatchAndMapOkResult<TResult, TResponse>(result, Mapper);
    }

    /// <summary>
    /// Creates a new entity.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public virtual async Task<IActionResult> Create([FromBody][Required] TAddRequest addRequest)
    {
        var command = Mapper.Map<TAddCommand>(addRequest!);
        var result = await Sender.Send(command);
        return MatchAndMapCreatedResult<bool>(result, Mapper);
    }

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<IActionResult> Update(
        [FromRoute][Required] string id,
        [FromBody][Required] TUpdateRequest updateRequest)
    {
        var command = Mapper.Map<TUpdateCommand>(updateRequest!);

        // Optional: set Id property if present
        var idProp = typeof(TUpdateCommand).GetProperty("Id");
        if (idProp != null && idProp.CanWrite)
        {
            idProp.SetValue(command, new Guid(id));
        }

        var result = await Sender.Send(command);
        return MatchAndMapNoContentResult<bool>(result, Mapper);
    }

    /// <summary>
    /// Deletes an entity by its ID.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<IActionResult> Delete([FromRoute][Required] string id)
    {
        var command = (TDeleteCommand)Activator.CreateInstance(typeof(TDeleteCommand), new Guid(id))!;
        var result = await Sender.Send(command);
        return MatchAndMapNoContentResult<bool>(result, Mapper);
    }
}
