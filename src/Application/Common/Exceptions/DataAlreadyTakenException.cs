using System.Net;

namespace JourneyMate.Application.Common.Exceptions;
public class DataAlreadyTakenException : JourneyMateException
{
	public DataAlreadyTakenException(string value, string type) : base($"{type} data: {value} already taken")
	{
	}

	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}
