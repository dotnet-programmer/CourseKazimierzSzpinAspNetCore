namespace GymManager.Application.Common.Events;

// eventy
// 3. interfejs, który będą implementowały klasy, które chcą zasubskrybować się na dany event, czyli klasy, których klasy zostaną wywołane po opublikowaniu eventu
// event będzie przekazywany jako typ generyczny i T musi być referencyjny i implementować interfejs-marker IEvent
public interface IEventHandler<T> where T : class, IEvent
{
	// ta metoda zostanie wywołana gdy event zostanie opublikowany
	// przekazywana jest klasa implementująca interfejs IEvent, czyli tutaj zostaną przekazane parametry eventu
	Task HandleAsync(T @event);
}