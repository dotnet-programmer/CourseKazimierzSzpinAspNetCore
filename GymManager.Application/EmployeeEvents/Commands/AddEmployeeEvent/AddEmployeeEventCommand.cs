using MediatR;

namespace GymManager.Application.EmployeeEvents.Commands.AddEmployeeEvent;

public class AddEmployeeEventCommand : IRequest
{
	public DateTime Start { get; set; }
	public DateTime? End { get; set; }
	public bool? IsFullDay { get; set; }
	public string UserId { get; set; }
}