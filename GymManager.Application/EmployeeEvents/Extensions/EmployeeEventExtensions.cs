using GymManager.Application.EmployeeEvents.Queries.GetEmployeeEvents;
using GymManager.Domain.Entities;

namespace GymManager.Application.EmployeeEvents.Extensions;

public static class EmployeeEventExtensions
{
	public static EmployeeEventDto ToEmployeeEventDto(this EmployeeEvent employeeEvent)
		=> employeeEvent == null ? 
			null :
			new EmployeeEventDto
			{
				Id = employeeEvent.EmployeeEventId,
				Start = employeeEvent.Start,
				End = employeeEvent.End,
				IsFullDay = employeeEvent.IsFullDay,
				Title = $"{employeeEvent.User.FirstName} {employeeEvent.User.LastName}",
				UserId = employeeEvent.UserId
			};
}
