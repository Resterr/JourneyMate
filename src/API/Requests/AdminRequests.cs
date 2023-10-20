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
		group.MapGet("roles/{id}", async (ISender mediator, Guid id) =>
			{
				var request = new GetRolesForUser(id);
				var result = await mediator.Send(request);
				return Results.Ok(result);
			})
			.RequireAuthorization("admin")
			.Produces<List<string>>()
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status403Forbidden)
			.Produces(StatusCodes.Status404NotFound)
			.WithMetadata(new SwaggerOperationAttribute("Get roles for user"));

		group.MapPatch("grant/{id}", async (ISender mediator, Guid id) =>
			{
				var request = new GrantAdminRole(id);
				await mediator.Send(request);
				return Results.Ok();
			})
			.RequireAuthorization("super-admin")
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status403Forbidden)
			.Produces(StatusCodes.Status404NotFound)
			.WithMetadata(new SwaggerOperationAttribute("Grant user admin role"));

		group.MapPatch("remove/{id}", async (ISender mediator, Guid id) =>
			{
				var request = new RemoveAdminRole(id);
				await mediator.Send(request);
				return Results.Ok();
			})
			.RequireAuthorization("super-admin")
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status403Forbidden)
			.Produces(StatusCodes.Status404NotFound)
			.WithMetadata(new SwaggerOperationAttribute("Remove user from admin role"));

		return group;
	}
}