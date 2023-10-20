using System.Net;
using FluentValidation.Results;

namespace JourneyMate.Application.Common.Exceptions;

public class ValidationException : JourneyMateException
{
	public IDictionary<string, string[]> Errors { get; }

	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

	public ValidationException() : base("One or more validation failures have occurred.")
	{
		Errors = new Dictionary<string, string[]>();
	}

	public ValidationException(IEnumerable<ValidationFailure> failures) : this()
	{
		Errors = failures.GroupBy(e => e.PropertyName, e => e.ErrorMessage)
			.ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
	}
}