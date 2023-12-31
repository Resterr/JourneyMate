using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class PlaceNotFoundException : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
	
	public PlaceNotFoundException() : base("Places not found") { }
	public PlaceNotFoundException(Guid id) : base($"Place with id: {id} not found") { }
	public PlaceNotFoundException(string value, string type) : base($"Place with {type}: {value} not found") { }
}