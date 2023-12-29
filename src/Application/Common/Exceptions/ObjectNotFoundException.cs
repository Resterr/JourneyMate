using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class ObjectNotFoundException : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
	public ObjectNotFoundException(string objectName) : base($"{objectName} not found") { }
	public ObjectNotFoundException(string objectName, string objectValue) : base($"{objectName} with value: {objectValue} not found") { }
}