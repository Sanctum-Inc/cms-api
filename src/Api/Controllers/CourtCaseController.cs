using Application.Common.Models;
using Application.CourtCase.Commands.Add;
using Application.CourtCase.Commands.Delete;
using Application.CourtCase.Commands.Update;
using Application.CourtCase.Queries.Get;
using Application.CourtCase.Queries.GetById;
using Application.CourtCase.Queries.GetCaseNumbers;
using Application.CourtCase.Queries.GetCourseCaseInformation;
using Contracts.CourtCases.Requests;
using Contracts.CourtCases.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[ProducesErrorResponseType(typeof(ProblemDetails))]
public class CourtCaseController : ApiControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISender _sender;

    public CourtCaseController(IMapper mapper, ISender sender)
    {
        _sender = sender;
        _mapper = mapper;
    }

    // GET /api/CourtCase
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CourtCaseResponse>), StatusCodes.Status200OK)]
    [EndpointName("GetAllCourtCases")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _sender.Send(new GetQuery());

        return MatchAndMapOkResult<IEnumerable<CourtCaseResult>, IEnumerable<CourtCaseResponse>>(result, _mapper);
    }


    // GET /api/CourtCase/case-numbers
    [HttpGet("case-numbers")]
    [ProducesResponseType(typeof(IEnumerable<CourtCaseNumberResponse>), StatusCodes.Status200OK)]
    [EndpointName("GetAllCaseNumbers")]
    public async Task<IActionResult> GetAllCaseNumbers()
    {
        var result = await _sender.Send(new GetCaseNumbersQuery());

        return MatchAndMapOkResult<IEnumerable<CourtCaseNumberResult>, IEnumerable<CourtCaseNumberResponse>>(result,
            _mapper);
    }

    // GET /api/CourtCase/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CourtCaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("GetCourtCasesById")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _sender.Send(new GetByIdCommand(id));

        var mapped = _mapper.Map<CourtCaseResponse>(result);

        return MatchAndMapOkResult<CourtCaseResult, CourtCaseResponse>(result, _mapper);
    }

    // GET /api/court-case-information/{id}
    [HttpGet("court-case-information/{id}")]
    [ProducesResponseType(typeof(CourtCaseInformationResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("GetCourtCaseInformation")]
    public async Task<IActionResult> GetCourtCaseInformation(Guid id)
    {
        var result = await _sender.Send(new GetCourtCaseInformationQuery(id));

        return MatchAndMapOkResult<CourtCaseInformationResult, CourtCaseInformationResponse>(result, _mapper);
    }

    // POST /api/CourtCase
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [EndpointName("CreateCourtCases")]
    public async Task<IActionResult> Create([FromBody] AddCourtCaseRequest request)
    {
        var command = _mapper.Map<AddCommand>(request);

        var created = await _sender.Send(command);

        return MatchAndMapCreatedResult(created, _mapper);
    }

    // PUT /api/CourtCase/{id}
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("UpdateCourtCases")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCourtCaseRequest request)
    {
        var command = _mapper.Map<UpdateCommand>(request) with { Id = id };

        var updated = await _sender.Send(command);

        return MatchAndMapNoContentResult(updated, _mapper);
    }

    // DELETE /api/CourtCase/{id}
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointName("DeleteCourtCases")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteCommand(id);

        var success = await _sender.Send(command);

        return MatchAndMapNoContentResult(success, _mapper);
    }
}
