using System.Diagnostics;
using GymManager.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GymManager.Application.Common.Behaviors;

// badanie wydajności aplikacji
public class PerformanceBehavior<TRequest, TResponse>(ILogger<TRequest> logger, ICurrentUserService currentUserService) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
	private readonly Stopwatch _timer = new();

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		_timer.Start();
		var response = await next();
		_timer.Stop();

		if (_timer.ElapsedMilliseconds > 500)
		{
			logger.LogInformation(
				"GymManager Long Running Request: {@Name} ({@ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
				typeof(TRequest).Name,
				_timer.ElapsedMilliseconds,
				currentUserService.UserId ?? string.Empty,
				currentUserService.UserName ?? string.Empty,
				request);
		}

		return response;
	}
}