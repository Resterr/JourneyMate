using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

internal class InvalidUserCredentialsException : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

	public InvalidUserCredentialsException() : base("Invalid user credentials") { }
}