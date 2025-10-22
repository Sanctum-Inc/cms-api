using System.ComponentModel.DataAnnotations;
using Application.Document.Commands.Add;
using Application.Document.Commands.Delete;
using Application.Document.Commands.Update;
using Application.Document.Queries.Download;
using Application.Document.Queries.Get;
using Application.Document.Queries.GetById;
using Application.Common.Models;
using Contracts.Documents.Requests;
using Contracts.Documents.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Controllers;

/// <summary>
/// Manages document upload, retrieval, update, and deletion.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DocumentController
    : CrudControllerBase<
        DocumentResult,                   // TResult
        DocumentResponse,                 // TResponse
        GetCommand,                       // TGetCommand
        GetByIdCommand,                   // TGetByIdCommand
        Object,               // TAddRequest
        AddCommand,                       // TAddCommand
        UpdateDocumentRequest,            // TUpdateRequest
        UpdateCommand,                    // TUpdateCommand
        DeleteCommand                     // TDeleteCommand
    >
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public DocumentController(IMapper mapper, ISender sender)
        : base(mapper, sender)
    {
        _sender = sender;
        _mapper = mapper;
    }

    public override async Task<IActionResult> Create([FromBody][Required] Object addRequest)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    /// <summary>
    /// Uploads a new document with a specific name.
    /// </summary>
    /// <param name="file">The file to upload.</param>
    /// <param name="name">The name to assign to the uploaded document.</param>
    /// <remarks>
    /// Sample request:
    ///
    /// POST /api/document
    /// FormData:
    /// file: myfile.pdf
    /// name: "Project Proposal"
    /// </remarks>
    /// <returns>Information about the created document.</returns>
    [HttpPost("upload")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upload(
        [FromForm] IFormFile file,
        [FromForm][Required] string name,
        [FromForm][Required] string caseId)
    {
        var command = new AddCommand(file, name, new Guid(caseId));

        var result = await _sender.Send(command);

        return MatchAndMapCreatedResult<bool>(result, _mapper);
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

        if (result.Value == null)
            return NotFound();

        return File(result.Value.Stream, result.Value.ContentType, result.Value.FileName);
    }
}
