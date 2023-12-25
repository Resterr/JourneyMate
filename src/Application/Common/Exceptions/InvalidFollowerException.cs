using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class InvalidFollowerException : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
	public InvalidFollowerException(string userName) : base($"User {userName} is not follower") { }
}