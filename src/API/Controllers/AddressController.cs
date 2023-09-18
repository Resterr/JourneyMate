using JourneyMate.Application.Features.AddressFeature.Commands;
using JourneyMate.Application.Features.AddressFeature.Queries;
using Microsoft.AspNetCore.Mvc;

namespace JourneyMate.API.Controllers;
public class AddressController : ApiBaseController
{
	[HttpGet]
	public async Task<IActionResult> GetAllAddresses([FromQuery] GetAllAddresses request)
	{
		var result = await Mediator.Send(request);
		return Ok(result);
	}

	[HttpPost("add")]
	public async Task<IActionResult> AddAddress([FromBody] AddAddress request)
	{
		await Mediator.Send(request);
		return Ok();
	}

	[HttpDelete("remove")]
	public async Task<IActionResult> RemoveAddress([FromBody] RemoveAddress request)
	{
		await Mediator.Send(request);
		return NoContent();
	}
}