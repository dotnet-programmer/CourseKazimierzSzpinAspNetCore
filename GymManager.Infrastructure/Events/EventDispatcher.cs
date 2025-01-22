using GymManager.Application.Common.Events;
using Microsoft.Extensions.DependencyInjection;

namespace GymManager.Infrastructure.Events;

public class EventDispatcher(IServiceProvider serviceProvider) : IEventDispatcher
{
	// pobranie wszystkich instancji które implementują IEventHandler i wywołanie ich
	// czyli wszystkie metody które podpięły się pod event zostały wywołane w momencie gdy zostanie opublikowany dany event
	public async Task PublishAsync<T>(T @event) where T : class, IEvent
	{
		// pobranie instancji
		using var scope = serviceProvider.CreateScope();
		var handlers = scope.ServiceProvider.GetServices<IEventHandler<T>>();

		// wywołanie metod
		var tasks = handlers.Select(x => x.HandleAsync(@event));

		// wywołanie wszystkich zdarzeń na raz i poczekanie aż zostaną zakończone i dopiero wtedy wyjście z metody
		await Task.WhenAll(tasks);
	}
}