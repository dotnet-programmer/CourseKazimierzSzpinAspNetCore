using GymManager.Application.Common.Interfaces;
using MediatR;

namespace GymManager.Application.EmployeeEvents.Commands.AddEmployeeEvent;

public class AddEmployeeEventCommandHandler(IApplicationDbContext context, IDateTimeService dateTimeService) : IRequestHandler<AddEmployeeEventCommand>
{
	public async Task Handle(AddEmployeeEventCommand request, CancellationToken cancellationToken)
	{
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