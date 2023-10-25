using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class AccessForbiddenException : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.Forbidden;

	public AccessForbiddenException() : base("Access forbidden") { }
}