using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Handles operations related to invoices.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ControllerBase
{
    /// <summary>
    /// Gets a status message for the InvoiceController.
    /// </summary>
    /// <returns>Status message.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        return Ok("InvoiceController is working!");
    }

    /// <summary>
    /// Gets an invoice by its ID.
    /// </summary>
    /// <param name="id">The ID of the invoice.</param>
    /// <returns>The invoice with the specified ID.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById([FromRoute][Required] int id)
    {
        return Ok($"InvoiceController received ID: {id}");
    }

    /// <summary>
    /// Creates a new invoice.
    /// </summary>
    /// <param name="invoice">The invoice object to create.</param>
    /// <returns>The created invoice.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody][Required] object invoice)
    {
        return CreatedAtAction(nameof(GetById), new { id = 1 }, invoice); // Assuming ID is 1 for demonstration
    }

    /// <summary>
    /// Updates an existing invoice.
    /// </summary>
    /// <param name="id">The ID of the invoice to update.</param>
    /// <param name="invoice">The updated invoice object.</param>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update([FromRoute][Required] int id, [FromBody][Required] object invoice)
    {
        return NoContent(); // Assuming update is successful
    }

    /// <summary>
    /// Deletes an invoice by its ID.
    /// </summary>
    /// <param name="id">The ID of the invoice to delete.</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete([FromRoute][Required] int id)
    {
        return NoContent(); // Assuming delete is successful
    }
}
