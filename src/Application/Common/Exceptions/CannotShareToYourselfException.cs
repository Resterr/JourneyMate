using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class CannotShareToYourselfException : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
	public CannotShareToYourselfException() : base("You cannot share to yourself") { }
}