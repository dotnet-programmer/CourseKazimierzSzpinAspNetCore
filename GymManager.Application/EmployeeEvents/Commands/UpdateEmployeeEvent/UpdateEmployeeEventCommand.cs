using MediatR;

namespace GymManager.Application.EmployeeEvents.Commands.UpdateEmployeeEvent;

public class UpdateEmployeeEventCommand : IRequest
{
	public int Id { get; set; }
	public DateTime Start { get; set; }
	public DateTime? End { get; set; }
	public bool? IsFullDay { get; set; }
	public string UserId { get; set; }
}