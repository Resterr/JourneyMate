using JourneyMate.Application.Common.Exceptions;
using System.Text.Json;

namespace JourneyMate.API.Middlewares;
internal sealed class ErrorHandlingMiddleware : IMiddleware
{
    public ErrorHandlingMiddleware()
    {
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException exception)
        {
            context.Response.StatusCode = 400;
            context.Response.Headers.Add("content-type", "application/json");

            var response = new
            {
				Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
				Title = "Validation errors",
                Detail = exception.Message,
				exception.Errors,
			};

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
		catch (BadRequestException exception)
		{
			context.Response.StatusCode = 400;
			context.Response.Headers.Add("content-type", "application/json");

			var response = new
			{
				Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
				Title = "Bad request exception",
				Detail = exception.Message
			};

			var json = JsonSerializer.Serialize(response);
			await context.Response.WriteAsync(json);
		}
		catch (UnauthorizedAccessException exception)
		{
			context.Response.StatusCode = 401;
			context.Response.Headers.Add("content-type", "application/json");

			var response = new
			{
				Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
				Title = "Unauthorized access exception",
				Detail = exception.Message
			};

			var json = JsonSerializer.Serialize(response);
			await context.Response.WriteAsync(json);
		}
		catch (ForbiddenException exception)
		{
			context.Response.StatusCode = 403;
			context.Response.Headers.Add("content-type", "application/json");

			var response = new
			{
				Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3",
				Title = "Forbidden exception",
				Detail = exception.Message
			};

			var json = JsonSerializer.Serialize(response);
			await context.Response.WriteAsync(json);
		}
		catch (NotFoundException exception)
        {
			context.Response.StatusCode = 404;
			context.Response.Headers.Add("content-type", "application/json");

			var response = new
			{
				Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
				Title = "Not found exception",
				Detail = exception.Message
			};

			var json = JsonSerializer.Serialize(response);
			await context.Response.WriteAsync(json);
		}
        catch (Exception)
		{
            context.Response.StatusCode = 500;
            context.Response.Headers.Add("content-type", "application/json");

            var response = new
            {
                Title = "Internal Server Error",
                Detail = "Something went wrong"
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
