using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.Application.Tickets.Queries.GetPdfTicket;

public class GetPdfTicketQuery : IRequest<TicketPdfVm>
{
	public string TicketId { get; set; }
	public string UserId { get; set; }

	// przekazanie kontekstu z kontrolera
	public ActionContext Context { get; set; }
}