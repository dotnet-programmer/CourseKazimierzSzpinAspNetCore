using GymManager.Application.Common.Interfaces;
using GymManager.Application.Tickets.Commands.AddTicket;
using GymManager.Application.Tickets.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Tickets.Queries.GetAddTicket;

public class GetAddTicketQueryHandler : IRequestHandler<GetAddTicketQuery, AddTicketVm>
{
	private readonly IApplicationDbContext _context;
	private readonly IDateTimeService _dateTimeService;

	public GetAddTicketQueryHandler(IApplicationDbContext context, IDateTimeService dateTimeService)
	{
		_context = context;
		_dateTimeService = dateTimeService;
	}

	public async Task<AddTicketVm> Handle(GetAddTicketQuery request, CancellationToken cancellationToken) =>
		new AddTicketVm
		{
			Ticket = new AddTicketCommand
			{
				StartDate = _dateTimeService.Now,
				TicketTypeId = 1
			},

			AvailableTicketTypes = await _context.TicketTypes
				// Globalizacja - pobieranie odpowiednich tłumaczeń z bazy danych
				.Include(x => x.Translations.Where(y => y.Language.Key == request.Language))
				.ThenInclude(x => x.Language)
				.AsNoTracking()
				.Select(x => x.ToDto())
				.ToListAsync()
		};
}