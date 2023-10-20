using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class UserNotFoundException : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
	public UserNotFoundException() : base("User not found") { }

	public UserNotFoundException(Guid id) : base($"User with id: {id} not found") { }

	public UserNotFoundException(string value, string type) : base($"User with {type}: {value} not found") { }
}