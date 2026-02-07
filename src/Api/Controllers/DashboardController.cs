using Application.Common.Models;
using Application.Dashboard.Queries.GetDashboardInformation;
using Contracts.CourtCases.Responses;
using Contracts.Dashboard.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ApiControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISender _sender;

    public DashboardController(IMapper mapper, ISender sender)
    {
        _sender = sender;
        _mapper = mapper;
    }

    // GET /api/CourtCase
    [HttpGet]
    [ProducesResponseType(typeof(DashBoardResult), StatusCodes.Status200OK)]
    [EndpointName("GetDashboardInformation")]
    public async Task<IActionResult> GetDashboardInformation()
    {
        var result = await _sender.Send(new GetDashboardInformationQuery());

        return MatchAndMapOkResult<DashBoardResult, DashBoardResponse>(result, _mapper);
    }

}
