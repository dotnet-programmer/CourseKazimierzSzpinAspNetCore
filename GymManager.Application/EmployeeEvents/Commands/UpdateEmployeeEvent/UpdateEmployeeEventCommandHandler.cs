using GymManager.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.EmployeeEvents.Commands.UpdateEmployeeEvent;

public class UpdateEmployeeEventCommandHandler : IRequestHandler<UpdateEmployeeEventCommand>
{
	private readonly IApplicationDbContext _context;

	public UpdateEmployeeEventCommandHandler(IApplicationDbContext context) =>
		_context = context;

	public async Task Handle(UpdateEmployeeEventCommand request, CancellationToken cancellationToken)
	{
		var employeeEvent = await _context
			.EmployeeEvents
			.FirstOrDefaultAsync(x => x.EmployeeEventId == request.Id);

		if (request.IsFullDay.GetValueOrDefault())
		{
			request.End = null;
		}

		employeeEvent.End = request.End;
		employeeEvent.IsFullDay = request.IsFullDay.GetValueOrDefault();
		employeeEvent.Start = request.Start;
		employeeEvent.UserId = request.UserId;

		await _context.SaveChangesAsync(cancellationToken);

		return;
	}
}