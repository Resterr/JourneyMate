using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class AddressNotFoundException : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
	public AddressNotFoundException() : base($"Address not found") { }
	public AddressNotFoundException(Guid id) : base($"Address with id: {id} not found") { }

	public AddressNotFoundException(string value, string type) : base($"Address with {type}: {value} not found") { }
}