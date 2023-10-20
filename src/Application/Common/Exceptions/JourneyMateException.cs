using System.Net;

namespace JourneyMate.Application.Common.Exceptions;

public abstract class JourneyMateException : Exception
{
	public abstract HttpStatusCode StatusCode { get; }
	protected JourneyMateException(string message) : base(message) { }
}