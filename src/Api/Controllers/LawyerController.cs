using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Api.Controllers;

/// <summary>
/// Handles operations related to lawyers.
/// </summary>
[ApiController]
[Route("[controller]")]
public class LawyerController : ControllerBase
{
    /// <summary>
    /// Gets a status message for the LawyerController.
    /// </summary>
    /// <returns>Status message.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        return Ok("LawyerController is working!");
    }

    /// <summary>
    /// Gets a lawyer by its ID.
    /// </summary>
    /// <param name="id">The ID of the lawyer.</param>
    /// <returns>The lawyer with the specified ID.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById([FromRoute][Required] int id)
    {
        return Ok($"LawyerController received ID: {id}");
    }

    /// <summary>
    /// Creates a new lawyer.
    /// </summary>
    /// <param name="lawyer">The lawyer object to create.</param>
    /// <returns>The created lawyer.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody][Required] object lawyer)
    {
        return CreatedAtAction(nameof(GetById), new { id = 1 }, lawyer); // Assuming ID is 1 for demonstration
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
    public IActionResult Update([FromRoute][Required] int id, [FromBody][Required] object lawyer)
    {
        return NoContent(); // Assuming update is successful
    }

    /// <summary>
    /// Deletes a lawyer by its ID.
    /// </summary>
    /// <param name="id">The ID of the lawyer to delete.</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete([FromRoute][Required] int id)
    {
        return NoContent(); // Assuming delete is successful
    }
}
