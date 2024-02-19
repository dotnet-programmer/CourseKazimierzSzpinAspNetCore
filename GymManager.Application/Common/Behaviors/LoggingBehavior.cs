using GymManager.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GymManager.Application.Common.Behaviors;

// INFO - Logowanie wszystkich requestów aplikacji
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
	private readonly ILogger _logger;
	private readonly ICurrentUserService _currentUserService;

	public LoggingBehavior(ILogger<TRequest> logger, ICurrentUserService currentUserService)
	{
		_logger = logger;
		_currentUserService = currentUserService;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var requestName = typeof(TRequest).Name;

		// INFO - Pobieranie informacji o zalogowanym użytkowniku bez zapytań na bazie danych
		string userId = _currentUserService.UserId ?? string.Empty;
		string userName = _currentUserService.UserName ?? string.Empty;

		_logger.LogInformation($"Handling {requestName}");
		_logger.LogInformation("GymManager Request: {@Name} {@UserId} {@UserName} {@Request}", requestName, userId, userName, request);

		// przypisanie do responsa odpowiedzi obsłużonego requesta
		var response = await next();

		// informacja, że request został obsłużony
		_logger.LogInformation($"Handled {typeof(TResponse).Name}");

		// zwrócenie odpowiedzi
		return response;
	}
}