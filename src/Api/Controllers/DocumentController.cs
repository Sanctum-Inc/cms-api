using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Api.Controllers;

/// <summary>
/// Handles operations related to documents.
/// </summary>
[ApiController]
[Route("[controller]")]
public class DocumentController : ControllerBase
{
    /// <summary>
    /// Gets a status message for the DocumentController.
    /// </summary>
    /// <returns>Status message.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        return Ok("DocumentController is working!");
    }

    /// <summary>
    /// Gets a document by its ID.
    /// </summary>
    /// <param name="id">The ID of the document.</param>
    /// <returns>The document with the specified ID.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById([FromRoute][Required] int id)
    {
        return Ok($"DocumentController received ID: {id}");
    }

    /// <summary>
    /// Creates a new document.
    /// </summary>
    /// <param name="document">The document object to create.</param>
    /// <returns>The created document.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody][Required] object document)
    {
        return CreatedAtAction(nameof(GetById), new { id = 1 }, document); // Assuming ID is 1 for demonstration
    }

    /// <summary>
    /// Updates an existing document.
    /// </summary>
    /// <param name="id">The ID of the document to update.</param>
    /// <param name="document">The updated document object.</param>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update([FromRoute][Required] int id, [FromBody][Required] object document)
    {
        return NoContent(); // Assuming update is successful
    }

    /// <summary>
    /// Deletes a document by its ID.
    /// </summary>
    /// <param name="id">The ID of the document to delete.</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete([FromRoute][Required] int id)
    {
        return NoContent(); // Assuming delete is successful
    }
}
