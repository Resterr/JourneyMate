using JourneyMate.Application.Common.Models;
using JourneyMate.Application.Features.ScheduleFeature.Commands;
using JourneyMate.Application.Features.ScheduleFeature.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace JourneyMate.API.Requests;

internal static class ScheduleRequests
{
	public static WebApplication RegisterScheduleRequests(this WebApplication app)
	{
		app.MapGroup("api/schedule")
			.MapScheduleEndpoints()
			.WithTags("Schedule");

		return app;
	}

	private static RouteGroupBuilder MapScheduleEndpoints(this RouteGroupBuilder group)
	{
		group.MapPost("plan", async (ISender sender, [FromBody] GetSchedulesForPlanPaginated request) =>
			{
				var result = await sender.Send(request);
				return Results.Ok(result);
			})
			.RequireAuthorization("user")
			.Produces<PaginatedList<ScheduleDto>>()
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status400BadRequest)
			.WithMetadata(new SwaggerOperationAttribute("Get schedules for plan"));

		group.MapPut("", async (ISender sender, [FromBody] AddOrUpdateSchedule request) =>
			{
				await sender.Send(request);
				return Results.Ok();
			})
			.RequireAuthorization("user")
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status400BadRequest)
			.WithMetadata(new SwaggerOperationAttribute("Add or update schedule for plan"));

		return group;
	}
}