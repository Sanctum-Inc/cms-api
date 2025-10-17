using System.ComponentModel.DataAnnotations;
using Application.Document.Commands.Add;
using Application.Document.Commands.Delete;
using Application.Document.Commands.Update;
using Application.Document.Queries.Download;
using Application.Document.Queries.Get;
using Application.Document.Queries.GetById;
using Contracts.Documents.Requests;
using Contracts.Documents.Responses;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Manages document upload, retrieval, update, and deletion.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DocumentController : ApiControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public DocumentController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Uploads a new document with a specific name.
    /// </summary>
    /// <param name="file">The file to upload.</param>
    /// <param name="name">The name to assign to the uploaded document.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/document
    ///     FormData:
    ///       file: myfile.pdf
    ///       name: "Project Proposal"
    /// </remarks>
    /// <returns>Information about the created document.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(bool), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upload(
        [FromForm] IFormFile file,
        [FromForm][Required] string name,
        [FromForm][Required] string caseId)
    {
        var command = new AddCommand(file, name, caseId);

        var result = await _sender.Send(command);

        return MatchAndMapResult<bool, bool>(result, _mapper);
    }

    /// <summary>
    /// Updates the name of an existing document.
    /// </summary>
    /// <param name="id">The ID of the document to update.</param>
    /// <param name="request">The new name for the document.</param>
    /// <returns>No content on success.</returns>
    [HttpPut("{id}/name")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateName(
        [FromBody][Required] UpdateDocumentRequest request)
    {
        var command = _mapper.Map<UpdateCommand>(request);

        var result = await _sender.Send(command);

        return MatchAndMapResult<bool, bool>(result, _mapper);
    }

    /// <summary>
    /// Gets a file structure representation of all documents and their attributes.
    /// </summary>
    /// <returns>List of documents with metadata.</returns>
    [HttpGet("structure")]
    [ProducesResponseType(typeof(IEnumerable<GetDocumentResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStructure()
    {
        var command = new GetCommand();

        var result = await _sender.Send(command);

        return MatchAndMapResult<IEnumerable<GetDocumentResult>, IEnumerable<GetDocumentResponse>>(result, _mapper);
    }

    /// <summary>
    /// Gets metadata and attributes of a specific document.
    /// </summary>
    /// <param name="id">The document ID.</param>
    /// <returns>Document metadata and attributes.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetDocumentByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute][Required] string id)
    {
        var command = new GetByIdCommand(new Guid(id));

        var result = await _sender.Send(command);

        return MatchAndMapResult<GetDocumentByIdResult, GetDocumentByIdResponse>(result, _mapper);
    }

    /// <summary>
    /// Downloads the file content of a specific document.
    /// </summary>
    /// <param name="id">The document ID.</param>
    /// <returns>The file stream of the document.</returns>
    [HttpGet("{id}/download")]
    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Download([FromRoute][Required] string id)
    {
        var command = new DownloadCommand(new Guid(id));

        var result = await _sender.Send(command);

        if (result.IsError)
        {
            return Problem(detail: string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        // For downloads, we bypass MatchAndMapResult to return FileStream directly
        if (result.Value == null) return NotFound();

        return File(result.Value.Stream, result.Value.ContentType, result.Value.FileName);
    }

    /// <summary>
    /// Deletes a document by ID.
    /// </summary>
    /// <param name="id">The document ID.</param>
    /// <returns>No content on success.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute][Required] string id)
    {
        var command = new DeleteCommand(new Guid(id));

        var result = await _sender.Send(command);

        return MatchAndMapResult<bool, bool>(result, _mapper);
    }
}