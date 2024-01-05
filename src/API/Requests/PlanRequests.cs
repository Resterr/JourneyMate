using JourneyMate.Application.Common.Models;
using JourneyMate.Application.Features.PlanFeature.Commands;
using JourneyMate.Application.Features.PlanFeature.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace JourneyMate.API.Requests;

internal static class PlanRequests
{
	public static WebApplication RegisterPlanRequests(this WebApplication app)
	{
		app.MapGroup("api/plan")
			.MapPlanEndpoints()
			.WithTags("Plan");

		return app;
	}

	private static RouteGroupBuilder MapPlanEndpoints(this RouteGroupBuilder group)
	{
		group.MapGet("", async (ISender sender, [AsParameters] GetAllPlansForUserPaginated request) =>
			{
				var result = await sender.Send(request);
				return Results.Ok(result);
			})
			.RequireAuthorization("user")
			.Produces<PaginatedList<PlanDto>>()
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status400BadRequest)
			.WithMetadata(new SwaggerOperationAttribute("Get plans for current user"));

		group.MapGet("shared", async (ISender sender, [AsParameters] GetAllSharedPlansForUserPaginated request) =>
			{
				var result = await sender.Send(request);
				return Results.Ok(result);
			})
			.RequireAuthorization("user")
			.Produces<PaginatedList<PlanDto>>()
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status400BadRequest)
			.WithMetadata(new SwaggerOperationAttribute("Get shared plans for current user"));

		group.MapGet("{PlanId:guid}/places", async (ISender sender, [AsParameters] GetPlacesForPlanPaginated request) =>
			{
				var result = await sender.Send(request);
				return Results.Ok(result);
			})
			.RequireAuthorization("user")
			.Produces<PaginatedList<PlaceDto>>()
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status400BadRequest)
			.WithMetadata(new SwaggerOperationAttribute("Get places for plan"));

		group.MapGet("shared/{PlanId:guid}/places", async (ISender sender, [AsParameters] GetSharedPlacesForPlanPaginated request) =>
			{
				var result = await sender.Send(request);
				return Results.Ok(result);
			})
			.RequireAuthorization("user")
			.Produces<PaginatedList<PlaceDto>>()
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status400BadRequest)
			.WithMetadata(new SwaggerOperationAttribute("Get places for shared plan"));

		group.MapGet("names", async (ISender sender, [AsParameters] GetPlanNames request) =>
			{
				var result = await sender.Send(request);
				return Results.Ok(result);
			})
			.RequireAuthorization("user")
			.Produces<List<PlanNameDto>>()
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status400BadRequest)
			.WithMetadata(new SwaggerOperationAttribute("Get plan names for user"));

		group.MapGet("{PlanId:guid}/names", async (ISender sender, [AsParameters] GetPlacesNameForPlan request) =>
			{
				var result = await sender.Send(request);
				return Results.Ok(result);
			})
			.RequireAuthorization("user")
			.Produces<List<PlaceNameDto>>()
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status400BadRequest)
			.WithMetadata(new SwaggerOperationAttribute("Get places names for plan"));

		group.MapPut("", async (ISender sender, [FromBody] AddOrUpdatePlan request) =>
			{
				await sender.Send(request);
				return Results.Ok();
			})
			.RequireAuthorization("user")
			.Produces(StatusCodes.Status201Created)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status400BadRequest)
			.WithMetadata(new SwaggerOperationAttribute("Create or update plan"));

		group.MapPatch("", async (ISender sender, [FromBody] UpdatePlan request) =>
			{
				await sender.Send(request);
				return Results.Ok();
			})
			.RequireAuthorization("user")
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status400BadRequest)
			.WithMetadata(new SwaggerOperationAttribute("Update plan name"));

		group.MapDelete("", async (ISender sender, [FromBody] RemovePlan request) =>
			{
				await sender.Send(request);
				return Results.NoContent();
			})
			.RequireAuthorization("user")
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status400BadRequest)
			.WithMetadata(new SwaggerOperationAttribute("Remove plan"));

		group.MapDelete("places", async (ISender sender, [FromBody] RemovePlacesFromPlan request) =>
			{
				await sender.Send(request);
				return Results.NoContent();
			})
			.RequireAuthorization("user")
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status400BadRequest)
			.WithMetadata(new SwaggerOperationAttribute("Remove plan place"));

		group.MapPut("share", async (ISender sender, [FromBody] SharePlan request) =>
			{
				await sender.Send(request);
				return Results.Ok();
			})
			.RequireAuthorization("user")
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status400BadRequest)
			.WithMetadata(new SwaggerOperationAttribute("Share plan"));

		return group;
	}
}