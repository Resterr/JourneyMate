using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class InvalidResponseStatus : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
	public InvalidResponseStatus(string responseStatus) : base($"Invalid response status: {responseStatus}") { }
}