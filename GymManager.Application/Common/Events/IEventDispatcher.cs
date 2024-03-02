namespace GymManager.Application.Common.Events;

// helper, który umożliwia/jest odpowiedzialny za publikowanie eventów
public interface IEventDispatcher
{
	// wywołanie tej metody oznacza opublikowanie eventa
	Task PublishAsync<T>(T @event) where T : class, IEvent;
}