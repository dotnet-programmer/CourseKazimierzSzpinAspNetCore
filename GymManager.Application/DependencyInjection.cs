using System.Reflection;
using GymManager.Application.Common.Behaviors;
using MediatR;
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

		// INFO - Logowanie wszystkich requestów aplikacji
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

		// INFO - badanie wydajności aplikacji
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));

		return services;
	}
}