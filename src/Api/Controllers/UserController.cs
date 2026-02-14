using Application.Users.Commands.ConfirmEmailOtp;
using Application.Users.Commands.Login;
using Application.Users.Commands.Register;
using Application.Users.Commands.ResendConfirmEmailOtp;
using Application.Users.Queries;
using Contracts.User.Requests;
using Contracts.User.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ApiControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISender _sender;

    public UserController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [EndpointName("Login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var command = _mapper.Map<LoginCommand>(loginRequest);

        var result = await _sender.Send(command);

        return MatchAndMapOkResult<AuthenticationResult, LoginResponse>(result, _mapper);
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [EndpointName("Register")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        var command = _mapper.Map<RegisterCommand>(registerRequest);

        var result = await _sender.Send(command);

        return MatchAndMapNoContentResult(result, _mapper);
    }

    [HttpGet("{id}")]
    [EndpointName("GetById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [Authorize]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var query = new GetQuery(id);

        var result = await _sender.Send(query);

        return MatchAndMapOkResult<UserResult, UserResponse>(result, _mapper);
    }

    [HttpGet("confirm-email")]
    [EndpointName("ConfirmEmail")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] string email)
    {
        var query = new ConfirmEmailOtpCommand(token, email);

        var result = await _sender.Send(query);

        return Redirect(result.Value);
    }

    [HttpGet("confirm-email/resend/{email}")]
    [EndpointName("ResendConfirmEmail")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResendConfirmEmail([FromRoute] string email)
    {
        var query = new ResendConfirmEmailOtpCommand(email);

        var result = await _sender.Send(query);

        return MatchAndMapOkResult<bool, bool>(result, _mapper);
    }
}
