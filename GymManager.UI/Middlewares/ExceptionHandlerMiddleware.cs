using System.Net;
using System.Text.Json;

namespace GymManager.UI.Middlewares;

// globalna obsługa wyjątków przez poprzez middleware

// wstrzyknięcie requesta przez konstruktor
public class ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
{
	private readonly RequestDelegate _next = next;
	private readonly ILogger<ExceptionHandlerMiddleware> _logger = logger;

	public async Task Invoke(HttpContext context)
	{
		try
		{
			// wywołanie każdego requesta w bloku try-catch, dzięki czemu można przechwycić ewentualne błędy
			// czyli tutaj podpinamy własnego middleware
			await _next.Invoke(context);
		}
		catch (Exception exception)
		{
			// za Name będzie podstawiony context.Request.Path
			_logger.LogError(exception, "GymManager Request: Nieobsłużony wyjątek - Request {Name}", context.Request.Path);

			// metoda obsługująca wyjątek i zwracająca odpowiedź do klienta
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