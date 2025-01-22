using GymManager.Application.Common.Interfaces;
//using GymManager.Domain.Entities;
using MediatR;

namespace GymManager.Application.EmployeeEvents.Commands.AddEmployeeEvent;

public class AddEmployeeEventCommandHandler(IApplicationDbContext context, IDateTimeService dateTimeService) : IRequestHandler<AddEmployeeEventCommand>
{
	public async Task Handle(AddEmployeeEventCommand request, CancellationToken cancellationToken)
	{
		// jeśli zdarzenie całodniowe to data zakończenia będzie nullem
		//if (request.IsFullDay.GetValueOrDefault())
		//{
		//	request.End = null;
		//}
		//EmployeeEvent employeeEvent = new()
		//{
		//	End = request.End,
		//	IsFullDay = request.IsFullDay.GetValueOrDefault(),
		//	Start = request.Start,
		//	UserId = request.UserId,
		//	CreatedDate = dateTimeService.Now
		//};
		//context.EmployeeEvents.Add(employeeEvent);

		context.EmployeeEvents.Add(new()
		{
			End = request.IsFullDay.GetValueOrDefault() ? null : request.End,
			IsFullDay = request.IsFullDay.GetValueOrDefault(),
			Start = request.Start,
			UserId = request.UserId,
			CreatedDate = dateTimeService.Now
		});

		await context.SaveChangesAsync(cancellationToken);
	}
}