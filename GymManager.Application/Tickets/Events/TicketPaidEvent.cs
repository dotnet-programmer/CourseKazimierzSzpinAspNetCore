using GymManager.Application.Common.Events;

namespace GymManager.Application.Tickets.Events;

// klasa oznaczona markerem jako event, będzie przekazywać parametry
public class TicketPaidEvent : IEvent
{
	public string TicketId { get; set; }
	public string UserId { get; set; }
}