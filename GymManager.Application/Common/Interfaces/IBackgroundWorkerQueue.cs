namespace GymManager.Application.Common.Interfaces;

// serwis do zadań wykonywanych w tle
public interface IBackgroundWorkerQueue
{
	// dodanie do kolejki metody do wykonania, 
	// wywołanie danej metody w tle działajacej aplikacji
	void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem, string workItemDescription);

	// pobieranie metody z kolejki, która ma zostać wykonana
	// zwraca delegata Func<CancellationToken, Task>, czyli metodę która ma zostać wywołana
	Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
}