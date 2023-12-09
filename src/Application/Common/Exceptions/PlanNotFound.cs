using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class PlanNotFound : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
	
	public PlanNotFound(Guid id) : base($"Plan with id: {id} not found") { }
}