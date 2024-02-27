using GymManager.Application.Common.Interfaces;
using GymManager.Domain.Entities;
using MediatR;

namespace GymManager.Application.EmployeeEvents.Commands.AddEmployeeEvent;

public class AddEmployeeEventCommandHandler : IRequestHandler<AddEmployeeEventCommand>
{
	private readonly IApplicationDbContext _context;
	private readonly IDateTimeService _dateTimeService;

	public AddEmployeeEventCommandHandler(IApplicationDbContext context, IDateTimeService dateTimeService)
	{
		_context = context;
		_dateTimeService = dateTimeService;
	}

	public async Task Handle(AddEmployeeEventCommand request, CancellationToken cancellationToken)
	{
		// jeśli zdarzenie całodniowe to data zakończenia będzie nullem
		if (request.IsFullDay.GetValueOrDefault())
		{
			request.End = null;
		}

		var employeeEvent = new EmployeeEvent
		{
			End = request.End,
			IsFullDay = request.IsFullDay.GetValueOrDefault(),
			Start = request.Start,
			UserId = request.UserId,
			CreatedDate = _dateTimeService.Now
		};

		_context.EmployeeEvents.Add(employeeEvent);
		await _context.SaveChangesAsync(cancellationToken);

		return;
	}
}