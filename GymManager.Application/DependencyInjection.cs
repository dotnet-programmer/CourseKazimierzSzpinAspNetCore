using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace GymManager.Application;

public static class DependencyInjection
{
	// INFO - tutaj będą dodawane/rejestrowane różne serwisy z aplikacji, które będą wywołane w projekcie UI w Program.cs
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		// INFO - konfiguracja MediatR
		// przed MediatR 12.0.0
		//services.AddMediatR(Assembly.GetExecutingAssembly());
		// od MediatR 12.0.0
		services.AddMediatR(c => c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

		return services;
	}
}