using MediatR;

namespace GymManager.Application.EmployeeEvents.Commands.DeleteEmployeeEvent;

public class DeleteEmployeeEventCommand : IRequest
{
	public int Id { get; set; }
}