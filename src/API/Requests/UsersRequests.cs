using JourneyMate.Application.Common.Models;
using JourneyMate.Application.Features.UserFeature.Commands;
using JourneyMate.Application.Features.UserFeature.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace JourneyMate.API.Requests;
internal static class UsersRequests
{
	public static WebApplication RegisterUsersRequests(this WebApplication app)
	{
		app.MapGroup("api/users/")
			.MapUsersEndpoints()
			.WithTags("Users");

		return app;
	}

	private static RouteGroupBuilder MapUsersEndpoints(this RouteGroupBuilder group)
	{
		group.MapGet("{id}", async (ISender mediator, Guid id) =>
		{
			var request = new GetUserById(id);
			var result = await mediator.Send(request);
			return result != null ? Results.Ok(result) : Results.NotFound();
		}).RequireAuthorization("admin")
			.Produces<UserDto>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status403Forbidden)
			.Produces(StatusCodes.Status404NotFound)
			.WithMetadata(new SwaggerOperationAttribute("Get user by id"));

		group.MapGet("current", async (ISender mediator) =>
		{
			var request = new SelfGetUser();
			var result = await mediator.Send(request);
			return result != null ? Results.Ok(result) : Results.Unauthorized();
		}).RequireAuthorization("user")
			.Produces<UserDto>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status401Unauthorized)
			.WithMetadata(new SwaggerOperationAttribute("Get current user"));

		group.MapPost("register", async (ISender mediator, [FromBody] RegisterUser request) =>
		{
			await mediator.Send(request);
			return Results.Ok();
		}).AllowAnonymous()
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.WithMetadata(new SwaggerOperationAttribute("Sign up user"));

		group.MapPost("login", async (ISender mediator, [FromBody] LoginUser request) =>
		{
			var result = await mediator.Send(request);
			return Results.Ok(result);
		}).AllowAnonymous()
			.Produces<TokensDto>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status404NotFound)
			.WithMetadata(new SwaggerOperationAttribute("Sign in user"));

		group.MapPost("token/refresh", async (ISender mediator, [FromBody] TokenRefresh request) =>
		{
			var token = await mediator.Send(request);
			return Results.Ok(token);
		}).RequireAuthorization("user")
			.Produces<TokensDto>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.WithMetadata(new SwaggerOperationAttribute("Refresh token"));

		group.MapPatch("token/remove", async (ISender mediator, [FromBody] TokenRemove request) =>
		{
			await mediator.Send(request);
			return Results.NoContent();
		}).RequireAuthorization("user")
			.Produces(StatusCodes.Status204NoContent)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.WithMetadata(new SwaggerOperationAttribute("Remove refresh token"));

		return group;
	}
}

