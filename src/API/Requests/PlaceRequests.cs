using JourneyMate.Application.Common.Models;
using JourneyMate.Application.Features.PlaceFeature.Commands;
using JourneyMate.Application.Features.PlaceFeature.Queries;
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
		group.MapGet("photo/{placeId:guid}", async (ISender sender, [AsParameters] GetPhotoForPlace request) =>
			{
				var result = await sender.Send(request);
				return Results.Stream(result, "image/jpeg");
			})
			.RequireAuthorization("user")
			.Produces<Stream>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status403Forbidden)
			.Produces(StatusCodes.Status400BadRequest)
			.WithMetadata(new SwaggerOperationAttribute("Get photo for place"));
		
		group.MapPost("search", async (ISender sender, [FromBody] SearchPlaces request) =>
			{
				var results = await sender.Send(request);
				return Results.Ok(results);
			})
			.RequireAuthorization("user")
			.Produces<PlaceDto>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status403Forbidden)
			.Produces(StatusCodes.Status400BadRequest)
			.WithMetadata(new SwaggerOperationAttribute("Search places"));
		
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