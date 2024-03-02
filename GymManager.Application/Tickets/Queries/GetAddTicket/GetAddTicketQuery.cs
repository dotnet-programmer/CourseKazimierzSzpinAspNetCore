using MediatR;

namespace GymManager.Application.Tickets.Queries.GetAddTicket;

public class GetAddTicketQuery : IRequest<AddTicketVm>
{
	// przekazywany wybrany język
	public string Language { get; set; }
}