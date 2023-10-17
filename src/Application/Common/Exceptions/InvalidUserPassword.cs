using System.Net;

namespace JourneyMate.Application.Common.Exceptions;
public class InvalidUserPassword : JourneyMateException
{
	public InvalidUserPassword() : base($"Invalid password")
	{
	}

	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}
