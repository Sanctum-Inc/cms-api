using System.ComponentModel.DataAnnotations;
using Application.Document.Commands.Add;
using Application.Document.Commands.Delete;
using Application.Document.Commands.Update;
using Application.Document.Queries.Download;
using Application.Document.Queries.Get;
using Application.Document.Queries.GetById;
using Contracts.Documents.Requests;
using Contracts.Documents.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
///     Manages document upload, retrieval, update, and deletion.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DocumentController : ApiControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISender _sender;

    public DocumentController(IMapper mapper, ISender sender)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    ///     Uploads a new document with a specific name.
    /// </summary>
    /// <param name="file">The file to upload.</param>
    /// <param name="name">The name to assign to the uploaded document.</param>
    /// <remarks>
    ///     Sample request:
    ///     POST /api/document
    ///     FormData:
    ///     file: myfile.pdf
    ///     name: "Project Proposal"
    /// </remarks>
    /// <returns>Information about the created document.</returns>
    [HttpPost("upload")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [EndpointName("UploadDocument")]
    public async Task<IActionResult> Upload(
        [FromForm] IFormFile file,
        [FromForm] [Required] string name,
        [FromForm] [Required] string caseId)
    {
        var command = new AddCommand(file, name, new Guid(caseId));

        var result = await _sender.Send(command);

        return MatchAndMapCreatedResult(result, _mapper);
    }

    /// <summary>
    ///     Downloads the file content of a specific document.
    /// </summary>
    /// <param name="id">The document ID.</param>
    /// <returns>The file stream of the document.</returns>
    [HttpGet("{id}/download")]
    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("DownloadDocument")]
    public async Task<IActionResult> Download([FromRoute] [Required] string id)
    {
        var command = new DownloadCommand(new Guid(id));
        var result = await _sender.Send(command);

        if (result.IsError)
        {
            return Problem(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        if (result.Value == null)
        {
            return NotFound();
        }

        return File(result.Value.Stream, result.Value.ContentType, result.Value.FileName);
    }


    // GET /api/CourtCase
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DocumentResponse>), StatusCodes.Status200OK)]
    [EndpointName("GetAllDocument")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _sender.Send(new GetCommand());

        return MatchAndMapOkResult<IEnumerable<DocumentResult>, IEnumerable<DocumentResponse>>(result, _mapper);
    }

    // GET /api/CourtCase/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(DocumentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("GetDocumentById")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _sender.Send(new GetByIdCommand(id));

        return MatchAndMapOkResult<DocumentResult, DocumentResponse>(result, _mapper);
    }

    // PUT /api/CourtCase/{id}
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("UpdateDocument")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDocumentRequest request)
    {
        var command = _mapper.Map<UpdateCommand>(request) with { Id = id };

        var updated = await _sender.Send(command);

        return MatchAndMapNoContentResult(updated, _mapper);
    }

    // DELETE /api/CourtCase/{id}
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("DeleteDocument")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteCommand(id);

        var success = await _sender.Send(command);

        return MatchAndMapNoContentResult(success, _mapper);
    }
}
