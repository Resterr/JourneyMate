using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class InvalidTokenException : JourneyMateException
{
	public InvalidTokenException() : base($"Invalid token")
	{
	}

	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}
