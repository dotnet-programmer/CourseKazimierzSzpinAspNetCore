using GymManager.Application.Common.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GymManager.Infrastructure.Services;

// serwis, który będzie cały czas uruchomiony i wykonywał w tle zadania które są zakolejkowane
public class LongRunningService(IBackgroundWorkerQueue queue, ILogger<LongRunningService> logger) : BackgroundService
{
	private readonly ILogger _logger = logger;

	// metoda która startuje zaraz po starcie aplikacji,
	// dopóki nie zostanie anulowana to będzie pobierała z kolejki zadania do wykoniania w tle
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			try
			{
				// pobierz z kolejki zadanie do wykonania
				var workItem = await queue.DequeueAsync(stoppingToken);

				_logger.LogInformation("ExecuteAsync Start...");

				// wywołanie delegata, czyli metody pobranej z kolejki do wykonania w tle
				await workItem(stoppingToken);

				_logger.LogInformation("ExecuteAsync Stop...");
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, null);
			}
		}
	}
}