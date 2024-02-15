using GymManager.Application.Common.Interfaces;
using GymManager.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymManager.Infrastructure;

public static class DependencyInjection
{
	// INFO - tutaj będą dodawane/rejestrowane różne serwisy z aplikacji, które będą wywołane w projekcie UI w Program.cs
	// konfiguracja tego, że to EF Core będzie używane i wskazanie bazy danych
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("DefaultConnection");

		services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

		// ApplicationDbContext - klasa kontekstu, czyli tej dziedziczącej po DbContext
		services.AddDbContext<ApplicationDbContext>(options => options
			.UseSqlServer(connectionString)
			// dodane żeby można było podejrzeć parametry przekazywane do kwerend i komend
			.EnableSensitiveDataLogging());

		return services;
	}

	public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder appBuilder)
	{
		return appBuilder;
	}
}