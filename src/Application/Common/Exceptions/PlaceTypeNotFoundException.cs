using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class PlaceTypeNotFoundException : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
	public PlaceTypeNotFoundException(Guid id) : base($"Place type with id: {id} not found") { }
	public PlaceTypeNotFoundException(string name) : base($"Place type with name: {name} not found") { }
}