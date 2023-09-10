using JourneyMate.Application.Features.UserFeature.Commands.LoginUser;
using JourneyMate.Application.Features.UserFeature.Commands.RegisterUser;
using JourneyMate.Application.Features.UserFeature.Commands.TokenRefresh;
using JourneyMate.Application.Features.UserFeature.Commands.TokenRemove;
using JourneyMate.Application.Features.UserFeature.Queries.GetUserById;
using JourneyMate.Application.Features.UserFeature.Queries.SelfGetUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JourneyMate.API.Controllers;
[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
	private readonly IMediator _mediator;

	public UserController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetUserById(Guid id)
	{
		var request = new GetUserById(id);
		var result = await _mediator.Send(request);
		return Ok(result);
	}

	[HttpGet("current")]
	public async Task<IActionResult> SelfGetUser()
	{
		var request = new SelfGetUser();
		var result = await _mediator.Send(request);
		return Ok(result);
	}

	[HttpPost("register")]
	public async Task<IActionResult> RegisterUser([FromBody] RegisterUser request)
	{
		await _mediator.Send(request);
		return Ok();
	}

	[HttpPost("login")]
	public async Task<IActionResult> LoginUser([FromBody] LoginUser request)
	{
		var result = await _mediator.Send(request);
		return Ok(result);
	}

	[HttpPost("token/refresh")]
	public async Task<IActionResult> TokenRefresh([FromBody] TokenRefresh request)
	{
		var token = await _mediator.Send(request);
		return Ok(token);
	}

	[HttpPatch("token/remove")]
	public async Task<IActionResult> TokenRemove([FromBody] TokenRemove request)
	{
		await _mediator.Send(request);
		return Ok();
	}
}