using System.Reflection;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using MediatR;
using AuthorizeAttribute = JourneyMate.Application.Common.Security.AuthorizeAttribute;
using AllowAnonymousAttribute = JourneyMate.Application.Common.Security.AllowAnonymousAttribute;

namespace JourneyMate.Application.Common.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
	private readonly ICurrentUserService _currentUserService;
	private readonly IAuthorizationService _authorizationService;

	public AuthorizationBehaviour(ICurrentUserService currentUserService, IAuthorizationService authorizationService)
	{
		_currentUserService = currentUserService;
		_authorizationService = authorizationService;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var allowAnonymousAttributes = request.GetType().GetCustomAttributes<AllowAnonymousAttribute>();

		if (allowAnonymousAttributes.Any())
		{
			return await next();
		}

		var userIdFromContext = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		bool isParsable = Guid.TryParse(userIdFromContext, out Guid userId);

		if (!isParsable)
		{
			throw new BadRequestException("Invalid ID format.");
		}

		var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

		if (authorizeAttributes.Any())
		{
			var authorizeAttributesRoles = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Role));

			if (authorizeAttributesRoles.Any())
			{
				var authorized = false;

				foreach (var roles in authorizeAttributesRoles.Select(a => a.Role.Split(',')))
				{
					foreach (var role in roles)
					{
						var isInRole = await _authorizationService.AuthorizeUserAsync(userId, role);
						if (isInRole)
						{
							authorized = true;
							break;
						}
					}
				}

				if (!authorized)
				{
					throw new ForbiddenException();
				}

				return await next();
			}
			else
			{
				var userAuthenticated = await _authorizationService.AuthenticateUserAsync(userId);

				if (!userAuthenticated)
				{
					throw new UnauthorizedAccessException();
				}

				return await next();
			}
		}
		throw new UnauthorizedAccessException();
	}
}

