using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class InvalidGuidFormat : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
	public InvalidGuidFormat(string? value) : base($"Could not parse '{value}' as a GUID.") { }
}