using JourneyMate.Application.Features.UserFeature.Commands;
using JourneyMate.Application.Features.UserFeature.Queries;
using Microsoft.AspNetCore.Mvc;

namespace JourneyMate.API.Controllers;
public class UserController : ApiBaseController
{
	[HttpGet("{id}")]
	public async Task<IActionResult> GetUserById(Guid id)
	{
		var request = new GetUserById(id);
		var result = await Mediator.Send(request);
		return Ok(result);
	}

	[HttpGet("current")]
	public async Task<IActionResult> SelfGetUser()
	{
		var request = new SelfGetUser();
		var result = await Mediator.Send(request);
		return Ok(result);
	}

	[HttpPost("register")]
	public async Task<IActionResult> RegisterUser([FromBody] RegisterUser request)
	{
		await Mediator.Send(request);
		return Ok();
	}

	[HttpPost("login")]
	public async Task<IActionResult> LoginUser([FromBody] LoginUser request)
	{
		var result = await Mediator.Send(request);
		return Ok(result);
	}

	[HttpPost("token/refresh")]
	public async Task<IActionResult> TokenRefresh([FromBody] TokenRefresh request)
	{
		var token = await Mediator.Send(request);
		return Ok(token);
	}

	[HttpPatch("token/remove")]
	public async Task<IActionResult> TokenRemove([FromBody] TokenRemove request)
	{
		await Mediator.Send(request);
		return Ok();
	}
}