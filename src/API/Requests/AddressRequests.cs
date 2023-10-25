using JourneyMate.Application.Common.Models;
using JourneyMate.Application.Features.AddressFeature.Commands;
using JourneyMate.Application.Features.AddressFeature.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace JourneyMate.API.Requests;

internal static class AddressRequests
{
	public static WebApplication RegisterAddressRequests(this WebApplication app)
	{
		app.MapGroup("api/address/")
			.MapAddressEndpoints()
			.WithTags("Address");

		return app;
	}

	private static RouteGroupBuilder MapAddressEndpoints(this RouteGroupBuilder group)
	{
		group.MapGet("", async (ISender sender, [AsParameters] GetAllAddresses request) =>
			{
				var result = await sender.Send(request);
				return Results.Ok(result);
			})
			.AllowAnonymous()
			.Produces<AddressDto>()
			.WithMetadata(new SwaggerOperationAttribute("Get addresses"));

		group.MapPost("add", async (ISender sender, [FromBody] AddAddress request) =>
			{
				await sender.Send(request);
				return Results.Ok();
			})
			.RequireAuthorization("admin")
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status403Forbidden)
			.Produces(StatusCodes.Status400BadRequest)
			.WithMetadata(new SwaggerOperationAttribute("Add address location"));

		group.MapDelete("remove", async (ISender sender, [FromBody] RemoveAddress request) =>
			{
				await sender.Send(request);
				return Results.NoContent();
			})
			.RequireAuthorization("admin")
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status403Forbidden)
			.Produces(StatusCodes.Status404NotFound)
			.WithMetadata(new SwaggerOperationAttribute("Remove address location"));

		return group;
	}
}