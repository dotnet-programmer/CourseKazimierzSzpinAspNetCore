using GymManager.Application.Tickets.Queries.GetAddTicket;
using GymManager.Domain.Entities;

namespace GymManager.Application.Tickets.Extensions;

public static class TicketTypeExtensions
{
	public static TicketTypeDto ToDto(this TicketType ticket) =>
		ticket == null ?
		null :
		new TicketTypeDto
		{
			Id = ticket.TicketTypeId,
			Price = ticket.Price,
			TicketTypeEnum = ticket.TicketTypeEnum,
			Name = ticket.Translations.FirstOrDefault()?.Name
		};
}