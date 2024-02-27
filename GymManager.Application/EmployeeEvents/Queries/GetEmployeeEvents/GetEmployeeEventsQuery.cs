using MediatR;

namespace GymManager.Application.EmployeeEvents.Queries.GetEmployeeEvents;

// bez parametrów, zwrócenie wszystkich zdarzeń i wyświetlenie na kalendarzu
public class GetEmployeeEventsQuery : IRequest<IEnumerable<EmployeeEventDto>>
{
}