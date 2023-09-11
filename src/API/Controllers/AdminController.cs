using JourneyMate.Application.Features.AdminFeature.Commands;
using JourneyMate.Application.Features.AdminFeature.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JourneyMate.API.Controllers;
[Route("api/admin")]
[ApiController]
public class AdminController : ControllerBase
{
	private readonly IMediator _mediator;

	public AdminController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("roles/{id}")]
	public async Task<IActionResult> GetRolesForUser([FromRoute] Guid id)
	{
		var request = new GetRolesForUser(id);
		var result = await _mediator.Send(request);
		return Ok(result);
	}

	[HttpPatch("grant/{id}")]
	public async Task<IActionResult> GrantAdmin([FromRoute] Guid id)
	{
		var request = new GrantAdminRole(id);
		await _mediator.Send(request);
		return Ok();
	}

	[HttpPatch("remove/{id}")]
	public async Task<IActionResult> RemoveAdmin([FromRoute] Guid id)
	{
		var request = new RemoveAdminRole(id);
		await _mediator.Send(request);
		return Ok();
	}
}
