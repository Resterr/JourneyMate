using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class InvalidGuidFormatException : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
	public InvalidGuidFormatException(string? value) : base($"Could not parse '{value}' as a GUID.") { }
}