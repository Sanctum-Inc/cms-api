using Application.Common.Models;
using Application.Email.Commands.Add;
using Application.Email.Commands.Update;
using Application.Email.Queries.Get;
using Application.Email.Queries.GetById;
using Contracts.Email.Requests;
using Contracts.Email.Response;
using Domain.Emails;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
///     Manages email sending operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EmailController : ApiControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISender _sender;

    public EmailController(IMapper mapper, ISender sender)
    {
        _sender = sender;
        _mapper = mapper;
    }

    // GET /api/Email
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EmailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("GetEmails")]
    public async Task<IActionResult> Get([FromRoute] string id)
    {
        var query = new GetQuery();

        var result = await _sender.Send(query);

        return MatchAndMapOkResult<IEnumerable<EmailResult>, IEnumerable<EmailResponse>>(result, _mapper);
    }

    // GET /api/Email/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(EmailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("GetEmailById")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var query = new GetByIdQuery(new Guid(id));

        var result = await _sender.Send(query);

        return MatchAndMapOkResult<EmailResult, EmailResponse>(result, _mapper);
    }

    // POST /api/Email
    [HttpPost]
    [ProducesResponseType(typeof(IEnumerable<EmailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("CreateEmail")]
    public async Task<IActionResult> Create(AddEmailRequest addEmailRequest)
    {
        var query = _mapper.Map<AddCommand>(addEmailRequest);

        var result = await _sender.Send(query);

        return MatchAndMapOkResult<Guid, Guid>(result, _mapper);
    }

    // PUT /api/Email/{id}
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(IEnumerable<EmailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("UpdateEmail")]
    public async Task<IActionResult> Update([FromRoute] string id, EmailStatus status)
    {
        var query = new UpdateCommand(new Guid(id), status);

        var result = await _sender.Send(query);

        return MatchAndMapOkResult<bool, bool>(result, _mapper);
    }
}
