﻿namespace GymManager.Application.Tickets.Queries.GetTicketById;

// dedykowany obiekt DTO który będzie zwracany przez kwerendy
public class TicketDto
{
	public int TicketId { get; set; }
	public string Name { get; set; }
}