using GymManager.Application.Common.Interfaces;

namespace GymManager.Infrastructure.Services;

public class DateTimeService : IDateTimeService
{
	public DateTime Now => DateTime.UtcNow;
}