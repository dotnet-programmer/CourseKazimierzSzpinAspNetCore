using System.Net;
using System.Text.Json;

namespace GymManager.UI.Middlewares;

// INFO - globalna obsługa wyjątków przez poprzez middleware

public class ExceptionHandlerMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger _logger;

	// wstrzyknięcie requesta przez konstruktor
	public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
		_next = next;
		_logger = logger;
	}


	public async Task Invoke(HttpContext context)
	{
		try
		{
			// wywołanie requesta, dzięki czemu można przechwycić ewentualne błędy
			// czyli tutaj podpinamy własnego middleware gdzie w tym miejscu będzie przekazany cały request
			await _next.Invoke(context);
		}
		catch (Exception exception)
		{
			// za Name będzie podstawiony context.Request.Path
			_logger.LogError(exception, "GymManager Request: Nieobsłużony wyjątek - Request {Name}", context.Request.Path);
			await HandleExceptionAsync(context, exception).ConfigureAwait(false);
		}
	}

	// w przypadku błędu zostanie wysłana od razu odpowiedź
	private Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		// ustawienie typu odpowiedzi
		context.Response.ContentType = "application/json";
		
		// stawienie kodu błędu
		int statusCode = (int)HttpStatusCode.InternalServerError;
		
		// przygotowanie obiektu który zostanie zwrócony
		string result = JsonSerializer.Serialize(new
		{
			StatusCode = statusCode,
			ErrorMessage = exception.Message
		});

		// dodanie przekierowania na dedykowaną stronę błędu
		context.Response.Redirect($"{context.Request.Scheme}://{context.Request.Host}/Error");

		// zwrócenie widoku
		return context.Response.WriteAsync(result);
	}
}