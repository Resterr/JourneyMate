using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class PlanNotFoundException : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
	
	public PlanNotFoundException(Guid id) : base($"Plan with id: {id} not found") { }
}