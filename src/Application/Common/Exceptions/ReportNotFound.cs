using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public class ReportNotFound : JourneyMateException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
	
	public ReportNotFound(Guid id) : base($"Report with id: {id} not found") { }
}