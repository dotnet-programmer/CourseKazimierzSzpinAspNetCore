using GymManager.Application.Common.Models;
using MediatR;

namespace GymManager.Application.Tickets.Queries.GetClientsTickets;

// przekazanie danych z tabeli/kontrolera
public class GetClientsTicketsQuery : IRequest<PaginatedList<TicketBasicsDto>>
{
	public string UserId { get; set; }

	// przekazanie informacji o sortowaniu i wyszukiwaniu
	public string OrderInfo { get; set; }
	public string SearchValue { get; set; }

	public int PageNumber { get; set; }
	public int PageSize { get; set; }
}