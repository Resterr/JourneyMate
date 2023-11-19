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
		group.MapGet("report/{id:guid}", async (ISender sender, [AsParameters] GetReportById request) =>
			{
				var result = await sender.Send(request);
				return Results.Ok(result);
			})
			.RequireAuthorization("user")
			.Produces<ReportDto>()
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status403Forbidden)
			.Produces(StatusCodes.Status400BadRequest)
			.WithMetadata(new SwaggerOperationAttribute("Get report data"));
		
		group.MapGet("report", async (ISender sender, [AsParameters] GetAllReports request) =>
			{
				var result = await sender.Send(request);
				return Results.Ok(result);
			})
			.RequireAuthorization("user")
			.Produces<List<ReportListDto>>()
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status403Forbidden)
			.Produces(StatusCodes.Status400BadRequest)
			.WithMetadata(new SwaggerOperationAttribute("Get all reports data"));
		
		group.MapGet("photo/{placeId:guid}", async (ISender sender, [AsParameters] GetPhotoForPlace request) =>
			{
				var result = await sender.Send(request);
				return Results.Stream(result, "image/jpeg");
			})
			.RequireAuthorization("user")
			.Produces<Stream>()
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status403Forbidden)
			.Produces(StatusCodes.Status400BadRequest)
			.WithMetadata(new SwaggerOperationAttribute("Get photo for place"));
		
		group.MapPost("report/generate", async (ISender sender, [FromBody] GenerateReport request) =>
			{
				var id = await sender.Send(request);
				return Results.Created($"api/place/report/{id}", id);
			})
			.RequireAuthorization("user")
			.Produces(StatusCodes.Status201Created)
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