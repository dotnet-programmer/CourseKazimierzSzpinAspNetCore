using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GymManager.Application.Common.Behaviors;

// INFO - badanie wydajności aplikacji
public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
	private readonly ILogger _logger;
	private readonly Stopwatch _timer;

	public PerformanceBehavior(ILogger<TRequest> logger)
	{
		_logger = logger;
		_timer = new Stopwatch();
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		_timer.Start();
		var response = await next();
		_timer.Stop();
		var elapsedMilliseconds = _timer.ElapsedMilliseconds;
		if (elapsedMilliseconds > 500)
		{
			_logger.LogInformation("GymManager Long Running Request: {@Name} ({@ElapsedMilliseconds} milliseconds) {@Request}", typeof(TRequest).Name, elapsedMilliseconds, request);
		}
		return response;
	}
}