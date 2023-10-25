using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class UserHasNoRoleException : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

	public UserHasNoRoleException(Guid id, string roleName) : base($"User with id: {id} has no role {roleName}") { }
}