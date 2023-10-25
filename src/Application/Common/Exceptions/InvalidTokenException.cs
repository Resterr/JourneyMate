using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class InvalidTokenException : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
	public InvalidTokenException() : base("Invalid token") { }
}