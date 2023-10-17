using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class RoleNotFoundException : JourneyMateException
{
	public RoleNotFoundException(Guid id) : base($"Role with id: {id} not found")
	{
	}

	public RoleNotFoundException(string name) : base($"Role with name: {name} not found")
	{
	}

	public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
}
