using System.Collections.Concurrent;
using GymManager.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace GymManager.Infrastructure.Services;

// serwis do zadań wykonywanych w tle
public class BackgroundWorkerQueue(ILogger<BackgroundWorkerQueue> logger) : IBackgroundWorkerQueue
{
	private readonly ILogger<BackgroundWorkerQueue> _logger = logger;

	// kolejka bezpieczna wątkowo do przechowywania delegatów, czyli zadań do wykonania
	private readonly ConcurrentQueue<Func<CancellationToken, Task>> _workItems = [];

	// semafor umożliwiający dostęp tylko dla 1 wątku równocześnie
	private readonly SemaphoreSlim _semaphore = new(0);

	// dodanie do kolejki metody do wykonania, 
	// wywołanie danej metody w tle działajacej aplikacji
	public void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem, string workItemDescription)
	{
		// zaloguj info o tym co jest przekazywane
		_logger.LogInformation($"QueueBackgroundWorkItem Start... {workItemDescription}");

		ArgumentNullException.ThrowIfNull(workItem);

		// dodaj workItem do kolejki
		_workItems.Enqueue(workItem);

		// zwolnij wątek
		_semaphore.Release();
	}

	// pobieranie metody z kolejki, która ma zostać wykonana
	// zwraca delegata Func<CancellationToken, Task>, czyli metodę która ma zostać wywołana
	public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
	{
		// zablokuj wątek
		await _semaphore.WaitAsync(cancellationToken);

		// pobierz delegata
		_workItems.TryDequeue(out var workItem);

		// zwróć delegata
		return workItem;
	}
}