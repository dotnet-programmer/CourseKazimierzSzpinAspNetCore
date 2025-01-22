using GymManager.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.EmployeeEvents.Queries.GetEmployeeEvents;

public class GetEmployeeEventsQueryHandler(IApplicationDbContext context, IRandomService randomService) : IRequestHandler<GetEmployeeEventsQuery, IEnumerable<EmployeeEventDto>>
{
	public async Task<IEnumerable<EmployeeEventDto>> Handle(GetEmployeeEventsQuery request, CancellationToken cancellationToken)
	{
		var employeeEvents = await context.EmployeeEvents
			.Include(x => x.User)
			.AsNoTracking()
			.Select(x => new EmployeeEventDto
			{
				Id = x.EmployeeEventId,
				Start = x.Start,
				End = x.End,
				IsFullDay = x.IsFullDay,
				Title = $"{x.User.FirstName} {x.User.LastName}",
				UserId = x.UserId
			})
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
			item.ThemeColor = randomService.GetColor();
		}
	}
}