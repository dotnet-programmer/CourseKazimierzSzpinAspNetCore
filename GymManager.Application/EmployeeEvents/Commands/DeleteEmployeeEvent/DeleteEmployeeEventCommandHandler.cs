using GymManager.Application.Common.Interfaces;
using MediatR;

namespace GymManager.Application.EmployeeEvents.Commands.DeleteEmployeeEvent;

public class DeleteEmployeeEventCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteEmployeeEventCommand>
{
	public async Task Handle(DeleteEmployeeEventCommand request, CancellationToken cancellationToken)
	{
		context.EmployeeEvents.Remove(new() { EmployeeEventId = request.Id });
		await context.SaveChangesAsync(cancellationToken);
	}
}