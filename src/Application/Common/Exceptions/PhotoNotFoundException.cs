using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class PhotoNotFoundException : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;

	public PhotoNotFoundException(Guid id) : base($"Photo for place with id: {id} not found") { }
}