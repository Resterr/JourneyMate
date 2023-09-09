using System.Security.Claims;
using JourneyMate.Application.Common.Interfaces;

namespace JourneyMate.API.Services;
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ClaimsPrincipal Principal => _httpContextAccessor.HttpContext?.User ?? throw new UnauthorizedAccessException();
    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
}
