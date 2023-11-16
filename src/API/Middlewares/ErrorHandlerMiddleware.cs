using System.Collections.Concurrent;
using System.Text.Json;
using Humanizer;
using JourneyMate.Application.Common.Exceptions;

namespace JourneyMate.API.Middlewares;

internal sealed class ErrorHandlerMiddleware : IMiddleware
{
	private static readonly ConcurrentDictionary<Type, string> _codes = new();
	private readonly ILogger<ErrorHandlerMiddleware> _logger;

	public ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger)
	{
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next(context);
		}
		catch (ValidationException exception)
		{
			_logger.LogError("{ErrorCode} : {Message}", exception.StatusCode, exception.Message);
			context.Response.StatusCode = (int)exception.StatusCode;
			context.Response.Headers.Add("content-type", "application/json");

			var response = new
			{
				Code = GetErrorCode(exception),
				Detail = exception.Message,
				exception.Errors
			};

			var json = JsonSerializer.Serialize(response);
			await context.Response.WriteAsync(json);
		}
		catch (JourneyMateException exception)
		{
			_logger.LogError("{ErrorCode} : {Message}", exception.StatusCode, exception.Message);
			context.Response.StatusCode = (int)exception.StatusCode;
			context.Response.Headers.Add("content-type", "application/json");

			var response = new
			{
				Code = GetErrorCode(exception),
				Detail = exception.Message
			};

			var json = JsonSerializer.Serialize(response);
			await context.Response.WriteAsync(json);
		}
		catch (Exception exception)
		{
			_logger.LogError("{ErrorCode} : {Message}", 500, exception.Message);
			context.Response.StatusCode = 500;
			context.Response.Headers.Add("content-type", "application/json");

			var response = new
			{
				Code = "internal_server_error",
				Detail = "Something went wrong."
			};

			var json = JsonSerializer.Serialize(response);
			await context.Response.WriteAsync(json);
		}
	}

	private static string GetErrorCode(object exception)
	{
		var type = exception.GetType();
		return _codes.GetOrAdd(type, type.Name.Underscore()
			.Replace("_exception", string.Empty));
	}
}