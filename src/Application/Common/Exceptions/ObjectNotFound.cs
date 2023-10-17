using System.Net;

namespace JourneyMate.Application.Common.Exceptions;
public class ObjectNotFound : JourneyMateException
{
	public ObjectNotFound(string objectName) : base($"{objectName} not found")
	{
	}

	public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
}
