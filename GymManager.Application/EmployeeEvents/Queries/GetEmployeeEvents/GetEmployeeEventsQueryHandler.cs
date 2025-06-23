using GymManager.Application.Common.Interfaces;
using GymManager.Application.EmployeeEvents.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.EmployeeEvents.Queries.GetEmployeeEvents;

public class GetEmployeeEventsQueryHandler(IApplicationDbContext context, IRandomColorService randomColorService) : IRequestHandler<GetEmployeeEventsQuery, IEnumerable<EmployeeEventDto>>
{
	public async Task<IEnumerable<EmployeeEventDto>> Handle(GetEmployeeEventsQuery request, CancellationToken cancellationToken)
	{
		var employeeEvents = await context.EmployeeEvents
			.Include(x => x.User)
			.AsNoTracking()
			.Select(x => x.ToEmployeeEventDto())
			.ToListAsync(cancellationToken);

		// ustawienie kolorów dla kafelków
		SetColors(employeeEvents);

		return employeeEvents;
	}

	// to można zrobić na inne sposoby, np dodając pole w bazie i przechodując info o kolorze dla danego użytkownika
	// tutaj żeby było szybciej jest losowanie randomowego koloru
	private void SetColors(List<EmployeeEventDto> employeeEvents)
	{
		foreach (var item in employeeEvents)
		{
			item.ThemeColor = randomColorService.GetColor();
		}
	}
}