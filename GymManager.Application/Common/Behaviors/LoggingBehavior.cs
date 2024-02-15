using MediatR;
using Microsoft.Extensions.Logging;

namespace GymManager.Application.Common.Behaviors;

// INFO - Logowanie wszystkich requestów aplikacji
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
	private readonly ILogger _logger;

	public LoggingBehavior(ILogger<TRequest> logger)
	{
		_logger = logger;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var requestName = typeof(TRequest).Name;

		_logger.LogInformation($"Handling {requestName}");
		_logger.LogInformation("GymManager Request: {@Name} {@Request}", requestName, request);

		// przypisanie do responsa odpowiedzi obsłużonego requesta
		var response = await next();

		// informacja, że request został obsłużony
		_logger.LogInformation($"Handled {typeof(TResponse).Name}");

		// zwrócenie odpowiedzi
		return response;
	}
}