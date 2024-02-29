using System.Net;

namespace GymManager.WebApi.Middlewares;

// Globalna obsługa wyjątków
public class ExceptionHandlerMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger _logger;

	public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task Invoke(HttpContext context)
	{
		try
		{
			await _next.Invoke(context);
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "GymManager Request: Nieobsłużony wyjątek - Request {Name}", context.Request.Path);

			await HandleExceptionAsync(context, exception).ConfigureAwait(false);
		}
	}

	private Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
		return context.Response.WriteAsJsonAsync(new { error = exception.Message });
	}
}