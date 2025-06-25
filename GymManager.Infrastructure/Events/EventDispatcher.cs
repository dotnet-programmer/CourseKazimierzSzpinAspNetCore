using GymManager.Application.Common.Events;
using Microsoft.Extensions.DependencyInjection;

namespace GymManager.Infrastructure.Events;

// eventy
// 5. klasa - helper umożliwiająca publikowanie eventów
public class EventDispatcher(IServiceProvider serviceProvider) : IEventDispatcher
{
	// pobranie wszystkich instancji które implementują jakiś event i wywołanie ich
	// czyli wszystkie metody które podpięły się pod event zostały wywołane w momencie gdy zostanie opublikowany dany event
	public async Task PublishAsync<T>(T @event) where T : class, IEvent
	{
		// pobranie instancji które implementują wybrany event, tutaj IEventHandler<T>
		using var scope = serviceProvider.CreateScope();
		var handlers = scope.ServiceProvider.GetServices<IEventHandler<T>>();

		// wywołanie metod
		var tasks = handlers.Select(x => x.HandleAsync(@event));

		// wywołanie wszystkich zdarzeń na raz i poczekanie aż zostaną zakończone i dopiero wtedy wyjście z metody
		// jeżeli nie trzeba czekać na zakończnie wszystkich metod, to można wywołać bez await
		await Task.WhenAll(tasks);
	}
}