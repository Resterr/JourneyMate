using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class PlaceTypeNotFound : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
	public PlaceTypeNotFound(Guid id) : base($"Place type with id: {id} not found") { }
	public PlaceTypeNotFound(string name) : base($"Place type with name: {name} not found") { }
}