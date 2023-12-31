using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class InvalidResponseStatusException : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
	public InvalidResponseStatusException(string responseStatus) : base($"Invalid response status: {responseStatus}") { }
}