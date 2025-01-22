using System.Diagnostics;
using GymManager.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GymManager.Application.Common.Behaviors;

// badanie wydajności aplikacji
public class PerformanceBehavior<TRequest, TResponse>(ILogger<TRequest> logger, ICurrentUserService currentUserService) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
	private readonly Stopwatch _timer = new Stopwatch();

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		_timer.Start();
		var response = await next();
		_timer.Stop();
		var elapsedMilliseconds = _timer.ElapsedMilliseconds;
		if (elapsedMilliseconds > 500)
		{
			// Pobieranie informacji o zalogowanym użytkowniku bez zapytań na bazie danych
			string userId = currentUserService.UserId ?? string.Empty;
			string userName = currentUserService.UserName ?? string.Empty;
			logger.LogInformation(
				"GymManager Long Running Request: {@Name} ({@ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
				typeof(TRequest).Name, elapsedMilliseconds, userId, userName, request);
		}
		return response;
	}
}