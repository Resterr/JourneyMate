using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class CannotFollowYourselfException : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
	public CannotFollowYourselfException() : base("You cannot follow yourself") { }
}