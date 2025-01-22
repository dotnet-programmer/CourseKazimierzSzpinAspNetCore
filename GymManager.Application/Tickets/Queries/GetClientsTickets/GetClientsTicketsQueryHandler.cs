using System.Linq.Dynamic.Core;
using GymManager.Application.Common.Extensions;
using GymManager.Application.Common.Interfaces;
using GymManager.Application.Common.Models;
using GymManager.Application.Tickets.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Tickets.Queries.GetClientsTickets;

public class GetClientsTicketsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetClientsTicketsQuery, PaginatedList<TicketBasicsDto>>
{
	public async Task<PaginatedList<TicketBasicsDto>> Handle(GetClientsTicketsQuery request, CancellationToken cancellationToken)
	{
		var tickets = context.Tickets
			.Where(x => x.UserId == request.UserId)
			.AsNoTracking();

		// filtrowanie danych 
		//if (!string.IsNullOrWhiteSpace(request.SearchValue))
		//{
		//	tickets = tickets.Where(x => x.TicketId.Contains(request.SearchValue));
		//}

		// żeby OrderBy zadziałało, trzeba dodać NuGet - System.Linq.Dynamic.Core
		// oraz: using System.Linq.Dynamic.Core;
		tickets = !string.IsNullOrWhiteSpace(request.OrderInfo) ?
			tickets = tickets.OrderBy(request.OrderInfo) :
			tickets = tickets.OrderByDescending(x => x.EndDate);

		var paginatedList = await tickets
			.Include(x => x.Invoice)
			.Select(x => x.ToBasicsDto())
			.PaginatedListAsync(request.PageNumber, request.PageSize);

		return paginatedList;
	}
}