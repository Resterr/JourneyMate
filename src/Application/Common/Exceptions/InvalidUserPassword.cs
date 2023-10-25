using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class InvalidUserPassword : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
	public InvalidUserPassword() : base("Invalid password") { }
}