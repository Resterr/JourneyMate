using JourneyMate.Application.Features.PlaceFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace JourneyMate.API.Requests;

internal static class PlaceRequests
{
	public static WebApplication RegisterPlaceRequests(this WebApplication app)
	{
		app.MapGroup("api/place")
			.MapPlaceEndpoints()
			.WithTags("Place");

		return app;
	}

	private static RouteGroupBuilder MapPlaceEndpoints(this RouteGroupBuilder group)
	{
		group.MapPost("add", async (ISender sender, [FromBody] AddPlacesFromAddress request) =>
			{
				await sender.Send(request);
				return Results.Ok();
			})
			.RequireAuthorization("admin")
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status403Forbidden)
			.Produces(StatusCodes.Status400BadRequest)
			.WithMetadata(new SwaggerOperationAttribute("Add places"));

		return group;
	}
}