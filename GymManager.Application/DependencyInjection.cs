using System.Reflection;
using FluentValidation;
using GymManager.Application.Common.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GymManager.Application;

public static class DependencyInjection
{
	// tutaj będą dodawane/rejestrowane różne serwisy z aplikacji, które będą wywołane w projekcie UI w Program.cs
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		// konfiguracja MediatR
		// przed MediatR 12.0.0
		//services.AddMediatR(Assembly.GetExecutingAssembly());
		// od MediatR 12.0.0
		services.AddMediatR(c => c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

		// Logowanie wszystkich requestów aplikacji
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

		// badanie wydajności aplikacji
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));

		// dodanie FluentValidator do dependency injection (informacje o walidatorach)
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

		return services;
	}
}