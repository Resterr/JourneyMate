using JourneyMate.Application.Common.Models;
using JourneyMate.Application.Features.UserFeature.Commands;
using JourneyMate.Application.Features.UserFeature.Queries;
using MediatR;
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
		group.MapGet("current", async (ISender sender) =>
			{
				var request = new SelfGetUser();
				var result = await sender.Send(request);
				return Results.Ok(result);
			})
			.RequireAuthorization("user")
			.Produces<UserDto>()
			.Produces(StatusCodes.Status401Unauthorized)
			.WithMetadata(new SwaggerOperationAttribute("Get current user"));

		group.MapPost("register", async (ISender sender, RegisterUser request) =>
			{
				await sender.Send(request);
				return Results.Ok();
			})
			.AllowAnonymous()
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.WithMetadata(new SwaggerOperationAttribute("Sign up user"));

		group.MapPost("login", async (ISender sender, LoginUser request) =>
			{
				var result = await sender.Send(request);
				return Results.Ok(result);
			})
			.AllowAnonymous()
			.Produces<TokensDto>()
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status404NotFound)
			.WithMetadata(new SwaggerOperationAttribute("Sign in user"));

		group.MapPost("token/refresh", async (ISender sender, TokenRefresh request) =>
			{
				var token = await sender.Send(request);
				return Results.Ok(token);
			})
			.AllowAnonymous()
			.Produces<TokensDto>()
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.WithMetadata(new SwaggerOperationAttribute("Refresh token"));

		group.MapPatch("token/remove", async (ISender sender, TokenRemove request) =>
			{
				await sender.Send(request);
				return Results.NoContent();
			})
			.RequireAuthorization("user")
			.Produces(StatusCodes.Status204NoContent)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.WithMetadata(new SwaggerOperationAttribute("Remove refresh token"));

		return group;
	}
}