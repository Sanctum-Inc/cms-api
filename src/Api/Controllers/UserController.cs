using Application.Users.Commands.Login;
using Application.Users.Commands.Register;
using Application.Users.Queries;
using Contracts.User.Requests;
using Contracts.User.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UserController : ApiControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public UserController(ISender sender, IMapper mapper) : base(mapper, sender)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var command = _mapper.Map<LoginCommand>(loginRequest);

        var result = await _sender.Send(command);

        return MatchAndMapOkResult<AuthenticationResult, LoginResponse>(result, _mapper);
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        var command = _mapper.Map<RegisterCommand>(registerRequest);

        var result = await _sender.Send(command);

        return MatchAndMapNoContentResult<bool>(result, _mapper);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] string id)
    {
        var query = new GetQuery(id);

        var result = await _sender.Send(query);

        return MatchAndMapOkResult<UserResult, UserResponse>(result, _mapper);
    }
}
