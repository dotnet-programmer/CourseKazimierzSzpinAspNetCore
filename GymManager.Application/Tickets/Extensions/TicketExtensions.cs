using GymManager.Application.Tickets.Queries.GetClientsTickets;
using GymManager.Application.Tickets.Queries.GetPrintTicket;
using GymManager.Domain.Entities;

namespace GymManager.Application.Tickets.Extensions;

public static class TicketExtensions
{
	public static TicketBasicsDto ToBasicsDto(this Ticket ticket)
		=> ticket == null ?
		null :
		new TicketBasicsDto
		{
			EndDate = ticket.EndDate.ToLongDateString(),
			StartDate = ticket.StartDate.ToLongDateString(),
			IsPaid = ticket.IsPaid,
			InvoiceId = ticket.Invoice?.InvoiceId,
			Id = ticket.TicketId
		};

	public static PrintTicketDto ToPrintTicketDto(this Ticket ticket)
		=> ticket == null ?
		null :
		new PrintTicketDto
		{
			Id = ticket.TicketId,
			CompanyContactEmail = "mail@testowy.pl",
			CompanyContactPhone = "500 500 500",
			EndDate = ticket.EndDate,
			StartDate = ticket.StartDate,
			FullName = $"{ticket.User.FirstName} {ticket.User.LastName}",
			Image = "images/gym-logo.jpg"
		};
}