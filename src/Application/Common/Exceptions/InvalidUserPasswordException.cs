using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class InvalidUserPasswordException : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
	public InvalidUserPasswordException() : base("Invalid password") { }
}