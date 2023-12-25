using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class AlreadyFollowedException : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
	public AlreadyFollowedException(string userName) : base($"User {userName} already followed") { }
}