using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class NotFollowedException : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
	public NotFollowedException(string userName) : base($"User {userName} not followed") { }
}