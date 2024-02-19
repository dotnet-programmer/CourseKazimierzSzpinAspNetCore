using System.Diagnostics;
using GymManager.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GymManager.Application.Common.Behaviors;

// INFO - badanie wydajności aplikacji
public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
	private readonly ILogger _logger;
	private readonly Stopwatch _timer;
	private readonly ICurrentUserService _currentUserService;

	public PerformanceBehavior(ILogger<TRequest> logger, ICurrentUserService currentUserService)
	{
		_logger = logger;
		_timer = new Stopwatch();
		_currentUserService = currentUserService;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		_timer.Start();
		var response = await next();
		_timer.Stop();
		var elapsedMilliseconds = _timer.ElapsedMilliseconds;
		if (elapsedMilliseconds > 500)
		{
			// INFO - Pobieranie informacji o zalogowanym użytkowniku bez zapytań na bazie danych
			string userId = _currentUserService.UserId ?? string.Empty;
			string userName = _currentUserService.UserName ?? string.Empty;
			_logger.LogInformation(
				"GymManager Long Running Request: {@Name} ({@ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
				typeof(TRequest).Name, elapsedMilliseconds, userId, userName, request);
		}
		return response;
	}
}