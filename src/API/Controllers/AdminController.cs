using JourneyMate.Application.Features.AdminFeature.Commands;
using JourneyMate.Application.Features.AdminFeature.Queries;
using Microsoft.AspNetCore.Mvc;

namespace JourneyMate.API.Controllers;
public class AdminController : ApiBaseController
{
	[HttpGet("roles/{id}")]
	public async Task<IActionResult> GetRolesForUser([FromRoute] Guid id)
	{
		var request = new GetRolesForUser(id);
		var result = await Mediator.Send(request);
		return Ok(result);
	}

	[HttpPatch("grant/{id}")]
	public async Task<IActionResult> GrantAdmin([FromRoute] Guid id)
	{
		var request = new GrantAdminRole(id);
		await Mediator.Send(request);
		return Ok();
	}

	[HttpPatch("remove/{id}")]
	public async Task<IActionResult> RemoveAdmin([FromRoute] Guid id)
	{
		var request = new RemoveAdminRole(id);
		await Mediator.Send(request);
		return Ok();
	}
}
