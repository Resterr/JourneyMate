using JourneyMate.Application.Common.Models;
using JourneyMate.Application.Features.AdminFeature.Commands;
using JourneyMate.Application.Features.AdminFeature.Queries;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace JourneyMate.API.Requests;

internal static class AdminRequests
{
	public static WebApplication RegisterAdminRequests(this WebApplication app)
	{
		app.MapGroup("api/admin/")
			.MapAdminEndpoints()
			.WithTags("Admin");

		return app;
	}

	private static RouteGroupBuilder MapAdminEndpoints(this RouteGroupBuilder group)
	{
		group.MapGet("users", async (ISender dispatcher, [AsParameters] GetAllUsers request) =>
			{
				var result = await dispatcher.Send(request);
				return Results.Ok(result);
			})
			.RequireAuthorization("admin")
			.Produces<UserDto>()
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status403Forbidden)
			.WithMetadata(new SwaggerOperationAttribute("Get all users"));

		group.MapGet("users/{id:guid}", async (ISender dispatcher, Guid id) =>
			{
				var request = new GetUserById(id);
				var result = await dispatcher.Send(request);
				return Results.Ok(result);
			})
			.RequireAuthorization("admin")
			.Produces<UserDto>()
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status403Forbidden)
			.Produces(StatusCodes.Status404NotFound)
			.WithMetadata(new SwaggerOperationAttribute("Get user by id"));

		group.MapGet("role/user/{id:guid}", async (ISender sender, Guid id) =>
			{
				var request = new GetRolesForUser(id);
				var result = await sender.Send(request);
				return Results.Ok(result);
			})
			.RequireAuthorization("admin")
			.Produces<List<RoleDto>>()
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status403Forbidden)
			.Produces(StatusCodes.Status404NotFound)
			.WithMetadata(new SwaggerOperationAttribute("Get roles for user"));

		group.MapPatch("role/grant", async (ISender sender, GrantRole request) =>
			{
				await sender.Send(request);
				return Results.Ok();
			})
			.RequireAuthorization("admin")
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status403Forbidden)
			.Produces(StatusCodes.Status404NotFound)
			.WithMetadata(new SwaggerOperationAttribute("Grant role"));

		group.MapPatch("role/remove", async (ISender sender, RemoveRole request) =>
			{
				await sender.Send(request);
				return Results.Ok();
			})
			.RequireAuthorization("admin")
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status403Forbidden)
			.Produces(StatusCodes.Status404NotFound)
			.WithMetadata(new SwaggerOperationAttribute("Remove role"));
		
		group.MapPatch("reportNames", async (ISender sender, UpdateReportNames request) =>
			{
				await sender.Send(request);
				return Results.Ok();
			})
			.RequireAuthorization("admin")
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status403Forbidden)
			.Produces(StatusCodes.Status404NotFound)
			.WithMetadata(new SwaggerOperationAttribute("Update report names"));

		group.MapDelete("users/{id:guid}", async (ISender sender, Guid id) =>
			{
				var request = new RemoveUser(id);
				await sender.Send(request);
				return Results.NoContent();
			})
			.RequireAuthorization("admin")
			.Produces(StatusCodes.Status204NoContent)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status403Forbidden)
			.Produces(StatusCodes.Status404NotFound)
			.WithMetadata(new SwaggerOperationAttribute("Remove user"));

		return group;
	}
}