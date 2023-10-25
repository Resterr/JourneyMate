using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class UserHasRoleException : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

	public UserHasRoleException(Guid id, string roleName) : base($"User with id: {id} has role {roleName}") { }
}