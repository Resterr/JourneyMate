using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class PlaceNotFound : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
	
	public PlaceNotFound() : base("Places not found") { }
	public PlaceNotFound(Guid id) : base($"Place with id: {id} not found") { }
	public PlaceNotFound(string value, string type) : base($"Place with {type}: {value} not found") { }
}