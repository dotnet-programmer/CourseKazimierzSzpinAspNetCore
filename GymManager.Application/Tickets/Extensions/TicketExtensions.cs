using GymManager.Application.Tickets.Queries.GetClientsTickets;
using GymManager.Domain.Entities;

namespace GymManager.Application.Tickets.Extensions;

public static class TicketExtensions
{
	public static TicketBasicsDto ToBasicsDto(this Ticket ticket) =>
		ticket == null ?
		null :
		new TicketBasicsDto
		{
			EndDate = ticket.EndDate.ToLongDateString(),
			StartDate = ticket.StartDate.ToLongDateString(),
			IsPaid = ticket.IsPaid,
			InvoiceId = ticket.Invoice?.InvoiceId,
			Id = ticket.TicketId
		};
}