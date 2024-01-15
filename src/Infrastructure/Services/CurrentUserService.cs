using System.Security.Claims;
using JourneyMate.Application.Common;
using JourneyMate.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace JourneyMate.Infrastructure.Services;

internal sealed class CurrentUserService : ICurrentUserService
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public CurrentUserService(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public ClaimsPrincipal? Principal => _httpContextAccessor.HttpContext?.User;

	public Guid? UserId => Principal?.FindFirstValue(ClaimTypes.NameIdentifier)
		.ToGuid();
}