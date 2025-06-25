namespace GymManager.Application.Common.Events;

// eventy
// 2. helper, który umożliwia/jest odpowiedzialny za publikowanie eventów
public interface IEventDispatcher
{
	// wywołanie tej metody oznacza opublikowanie eventa
	Task PublishAsync<T>(T @event) where T : class, IEvent;
}