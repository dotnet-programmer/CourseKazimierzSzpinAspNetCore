using GymManager.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.EmployeeEvents.Commands.UpdateEmployeeEvent;

public class UpdateEmployeeEventCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateEmployeeEventCommand>
{
	public async Task Handle(UpdateEmployeeEventCommand request, CancellationToken cancellationToken)
	{
		var employeeEvent = await context.EmployeeEvents.FirstOrDefaultAsync(x => x.EmployeeEventId == request.Id, cancellationToken);
		employeeEvent.End = request.IsFullDay.GetValueOrDefault() ? null : request.End;
		employeeEvent.IsFullDay = request.IsFullDay.GetValueOrDefault();
		employeeEvent.Start = request.Start;
		employeeEvent.UserId = request.UserId;
		await context.SaveChangesAsync(cancellationToken);
	}
}