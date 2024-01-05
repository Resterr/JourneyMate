using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class AddressAlreadyAddedException : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
	public AddressAlreadyAddedException() : base("Address already added") { }
}