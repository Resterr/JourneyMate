using System.Security.Claims;

namespace JourneyMate.Application.Common.Interfaces;

public interface ICurrentUserService
{
	ClaimsPrincipal? Principal { get; }
	Guid? UserId { get; }
}