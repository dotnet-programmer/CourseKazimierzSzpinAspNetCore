using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GymManager.Infrastructure;

public static class DependencyInjection
{
	// INFO - tutaj będą dodawane/rejestrowane różne serwisy z aplikacji, które będą wywołane w projekcie UI w Program.cs
	public static IServiceCollection AddInfrastructure(this IServiceCollection services)
	{
		return services;
	}

	public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder appBuilder)
	{
		return appBuilder;
	}
}