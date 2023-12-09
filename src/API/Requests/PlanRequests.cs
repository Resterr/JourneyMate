﻿using JourneyMate.Application.Common.Models;
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
		
		group.MapGet("plan/{PlanId:guid}/items", async (ISender sender, [AsParameters] GetSchedulesForPlanPaginated request) =>
			{
				var result = await sender.Send(request);
				return Results.Ok(result);
			})
			.RequireAuthorization("user")
			.Produces<PaginatedList<PlanScheduleDto>>()
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status400BadRequest)
			.WithMetadata(new SwaggerOperationAttribute("Get schedules for plan"));
		
		group.MapPost("", async (ISender sender, [FromBody] AddPlan request) =>
			{
				await sender.Send(request);
				return Results.Ok();
			})
			.RequireAuthorization("user")
			.Produces(StatusCodes.Status201Created)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status400BadRequest)
			.WithMetadata(new SwaggerOperationAttribute("Create plan"));
		
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
		
		group.MapPatch("schedule", async (ISender sender, [FromBody] UpdateSchedule request) =>
			{
				await sender.Send(request);
				return Results.Ok();
			})
			.RequireAuthorization("user")
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status400BadRequest)
			.WithMetadata(new SwaggerOperationAttribute("Update schedule for plan"));
		
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

		return group;
	}
}