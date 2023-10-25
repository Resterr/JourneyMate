using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

internal class InvalidUserCredentials : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

	public InvalidUserCredentials() : base("Invalid user credentials") { }
}