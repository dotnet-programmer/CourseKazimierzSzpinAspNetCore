using MediatR;

namespace GymManager.Application.Tickets.Queries.GetAddTicket;

public class GetAddTicketQuery : IRequest<AddTicketVm>
{
	// Globalizacja - przekazywany wybrany język
	public string Language { get; set; }
}