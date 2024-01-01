using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class UserNotFollowedException : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
	public UserNotFollowedException(string userName) : base($"User {userName} not followed") { }
}