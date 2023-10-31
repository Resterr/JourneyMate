using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class AddressAlreadyAdded : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
	public AddressAlreadyAdded() : base($"Address already added") { }
}