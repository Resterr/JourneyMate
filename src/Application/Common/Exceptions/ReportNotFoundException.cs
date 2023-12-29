using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class ReportNotFoundException : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
	
	public ReportNotFoundException(Guid id) : base($"Report with id: {id} not found") { }
}