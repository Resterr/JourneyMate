using System.Net;

namespace JourneyMate.Application.Common.Exceptions;
public class AddressNotFound : JourneyMateException
{
	public AddressNotFound(Guid id) : base($"Address with id: {id} not found")
	{
	}

	public AddressNotFound(string value, string type) : base($"Address with {type}: {value} not found")
	{
	}


	public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
}
