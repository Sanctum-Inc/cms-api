using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Api.Controllers;

/// <summary>
/// Handles operations related to court cases.
/// </summary>
[ApiController]
[Route("[controller]")]
public class CourtCaseController : ControllerBase
{
    /// <summary>
    /// Gets a status message for the CourtCaseController.
    /// </summary>
    /// <returns>Status message.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        return Ok("CourtCaseController is working!");
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
    public IActionResult Create([FromBody][Required] object courtCase)
    {
        return CreatedAtAction(nameof(GetById), new { id = 1 }, courtCase); // Assuming ID is 1 for demonstration
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
