using JourneyMate.Application.Features.AddressFeature.Commands.AddAddress;
using JourneyMate.Application.Features.AddressFeature.Commands.RemoveAddress;
using JourneyMate.Application.Features.AddressFeature.Queries.GetAllAddresses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JourneyMate.API.Controllers;
[ApiController]
[Route("api/address")]
public class AddressController : ControllerBase
{
	private readonly IMediator _mediator;

	public AddressController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	public async Task<IActionResult> GetAllAddresses([FromQuery] GetAllAddresses request)
	{
		var result = await _mediator.Send(request);
		return Ok(result);
	}

	[HttpPost("add")]
	public async Task<IActionResult> AddAddress([FromBody] AddAddress request)
	{
		await _mediator.Send(request);
		return Ok();
	}

	[HttpDelete("remove")]
	public async Task<IActionResult> RemoveAddress([FromBody] RemoveAddress request)
	{
		await _mediator.Send(request);
		return NoContent();
	}
}