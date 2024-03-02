namespace GymManager.Application.Common.Interfaces;

public interface IBackgroundWorkerQueue
{
	// pobieranie metody z kolejki, która ma zostać wykonana
	// zwraca delegata Func<CancellationToken, Task>, czyli metodę która ma zostać wywołana
	Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);

	// dodanie do kolejki metody do wykonania
	void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem, string workItemDescription);
}