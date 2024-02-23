using GymManager.Application.Announcements.Extensions;
using GymManager.Application.Announcements.Queries.Dtos;
using GymManager.Application.Charts.Queries.Dtos;
using GymManager.Application.Common.Extensions;
using GymManager.Application.Common.Interfaces;
using GymManager.Application.Common.Models;
using GymManager.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Clients.Queries.GetClientDashboard;

public class GetClientDashboardQueryHandler : IRequestHandler<GetClientDashboardQuery, GetClientDashboardVm>
{
	private readonly IApplicationDbContext _context;
	private readonly IDateTimeService _dateTimeService;

	public GetClientDashboardQueryHandler(IApplicationDbContext context, IDateTimeService dateTimeService)
	{
		_context = context;
		_dateTimeService = dateTimeService;
	}

	public async Task<GetClientDashboardVm> Handle(GetClientDashboardQuery request, CancellationToken cancellationToken)
	{
		GetClientDashboardVm vm = new();
		var user = await GetUser(request);
		vm.Email = user.Email;
		var ticket = GetActiveTicket(user);
		vm.TicketEndDate = GetTicketEndDate(ticket);
		vm.Announcements = await GetAnnouncements(request);

		// w przykładzie dane wpisane na sztywno
		// żeby zrobić odpowiednią implementację, trzeba zrobić odpowiednią kwerendę na bazie danych
		// a następnie pogrupować w zależności od tego, co będzie wyświetlane
		var color = GetChartColors();
		var borderColors = GetChartBorderColors();
		vm.TrainingCountChart = GetTrainingCountChart(color, borderColors);
		vm.TheBestTrainingsChart = GetTheBestTrainingsChart(color, borderColors);

		return vm;
	}

	private async Task<ApplicationUser> GetUser(GetClientDashboardQuery request) =>
		await _context.Users
			.Include(x => x.Tickets)
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == request.UserId);

	private Ticket GetActiveTicket(ApplicationUser user) =>
		user.Tickets.FirstOrDefault(x => x.StartDate.Date <= _dateTimeService.Now.Date && x.EndDate.Date >= _dateTimeService.Now.Date);

	private static DateTime? GetTicketEndDate(Ticket ticket) => 
		ticket == null ? null : ticket.EndDate;

	private async Task<PaginatedList<AnnouncementDto>> GetAnnouncements(GetClientDashboardQuery request) =>
	await _context.Announcements
		.AsNoTracking()
		.OrderByDescending(x => x.Date)
		.Select(x => x.ToDto())
		.PaginatedListAsync(request.PageNumber, request.PageSize);

	private List<string> GetChartColors() => 
		[
			"rgba(255, 99, 132, 0.2)",
			"rgba(54, 162, 235, 0.2)",
			"rgba(255, 206, 86, 0.2)",
			"rgba(75, 192, 192, 0.2)",
			"rgba(153, 102, 255, 0.2)",
			"rgba(255, 159, 64, 0.2)"
		];

	private List<string> GetChartBorderColors() => 
		[
			"rgba(255, 99, 132, 1)",
			"rgba(54, 162, 235, 1)",
			"rgba(255, 206, 86, 1)",
			"rgba(75, 192, 192, 1)",
			"rgba(153, 102, 255, 1)",
			"rgba(255, 159, 64, 1)"
		];

	private ChartDto GetTrainingCountChart(List<string> colors, List<string> borderColors)
	{
		int i = 0;

		return new ChartDto
		{
			Label = "Ilość",
			Positions = new List<ChartPositionDto>
			{
				new ChartPositionDto { Label = "Styczeń", Data = 4, BorderColor = borderColors[i], Color = colors[i++] },
				new ChartPositionDto { Label = "Luty", Data = 10, BorderColor = borderColors[i], Color = colors[i++] },
				new ChartPositionDto { Label = "Marzec", Data = 5, BorderColor = borderColors[i], Color = colors[i++] },
				new ChartPositionDto { Label = "Kwiecień", Data = 6, BorderColor = borderColors[i], Color = colors[i++] },
				new ChartPositionDto { Label = "Maj", Data = 8, BorderColor = borderColors[i], Color = colors[i++] },
				new ChartPositionDto { Label = "Czerwiec", Data = 14, BorderColor = borderColors[i], Color = colors[i++] },
			}
		};
	}

	private ChartDto GetTheBestTrainingsChart(List<string> colors, List<string> borderColors)
	{
		int i = 0;

		return new ChartDto
		{
			Positions = new List<ChartPositionDto>
			{
				new ChartPositionDto { Label = "Siłownia", Data = 855, BorderColor = borderColors[i], Color = colors[i++] },
				new ChartPositionDto { Label = "Crossfit", Data = 600, BorderColor = borderColors[i], Color = colors[i++] },
				new ChartPositionDto { Label = "Basen", Data = 350, BorderColor = borderColors[i], Color = colors[i++] },
				new ChartPositionDto { Label = "Aerobik", Data = 100, BorderColor = borderColors[i], Color = colors[i++] },
				new ChartPositionDto { Label = "Trójbój", Data = 8, BorderColor = borderColors[i], Color = colors[i++] },
				new ChartPositionDto { Label = "Rower", Data = 444, BorderColor = borderColors[i], Color = colors[i++] },
			}
		};
	}
}