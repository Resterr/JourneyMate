using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class ObjectNotFound : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
	public ObjectNotFound(string objectName) : base($"{objectName} not found") { }
}