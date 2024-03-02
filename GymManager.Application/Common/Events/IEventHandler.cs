namespace GymManager.Application.Common.Events;

// interfejs, który będą implementowały klasy, które chcą zasubskrybować się na dany event
// event będzie przekazywany jako typ generyczny i musi on być referencyjny i implementować interfejs-marker IEvent
public interface IEventHandler<T> where T : class, IEvent
{
	// ta metoda zostanie wywołana gdy event zostanie opublikowany
	Task HandleAsync(T @event);
}