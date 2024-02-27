using GymManager.Application.Common.Interfaces;
using GymManager.Domain.Entities;
using MediatR;

namespace GymManager.Application.EmployeeEvents.Commands.DeleteEmployeeEvent;

public class DeleteEmployeeEventCommandHandler : IRequestHandler<DeleteEmployeeEventCommand>
{
	private readonly IApplicationDbContext _context;

	public DeleteEmployeeEventCommandHandler(IApplicationDbContext context) => 
		_context = context;

	public async Task Handle(DeleteEmployeeEventCommand request, CancellationToken cancellationToken)
	{
		_context.EmployeeEvents.Remove(new EmployeeEvent { EmployeeEventId = request.Id });
		await _context.SaveChangesAsync(cancellationToken);
		return;
	}
}