using GymManager.Application.Common.Events;

namespace GymManager.Application.Tickets.Events;

// eventy
// 4. klasa oznaczona markerem jako IEvent, będzie przekazywać parametry
public class TicketPaidEvent : IEvent
{
	public string TicketId { get; set; }
	public string UserId { get; set; }
}