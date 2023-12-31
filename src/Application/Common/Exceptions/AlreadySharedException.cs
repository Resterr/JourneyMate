using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class AlreadySharedException : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
	public AlreadySharedException(Guid planId, string userName) : base($"Plan id {planId} for {userName} already shared") { }
}