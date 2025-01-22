using GymManager.Application.Tickets.Commands.AddTicket;

namespace GymManager.Application.Tickets.Queries.GetAddTicket;

// ViewModel, bo do widoku poza modelem potrzeba jeszcze przekazać listę typów karnetów
public class AddTicketVm
{
	public AddTicketCommand Ticket { get; set; }
	public List<TicketTypeDto> AvailableTicketTypes { get; set; }
}