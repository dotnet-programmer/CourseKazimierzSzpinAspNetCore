using GymManager.Application.Common.Interfaces;
using GymManager.Infrastructure.Persistence;
using GymManager.Infrastructure.Services;
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
		// pobranie connection stringa z ustawień
		var connectionString = configuration.GetConnectionString("DefaultConnection");

		// dependency injection - używaj ApplicationDbContext wszędzie tam gdzie jest IApplicationDbContext
		services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

		// ApplicationDbContext - klasa kontekstu, czyli tej dziedziczącej po DbContext
		services.AddDbContext<ApplicationDbContext>(options => options
			// ustawienie żeby DbContext używał bazy danych Sql Server o podanym connection stringu
			.UseSqlServer(connectionString)
			// dodane żeby można było podejrzeć parametry przekazywane do kwerend i komend
			.EnableSensitiveDataLogging());

		// dependency injection - używaj AppSettingsService wszędzie tam gdzie jest IAppSettingsService, 
		// singleton, bo chcemy pracować tylko na 1 słowniku z ustawieniami
		services.AddSingleton<IAppSettingsService, AppSettingsService>();

		return services;
	}

	// tutaj będą wywoływane metody podczas startu aplikacji
	public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder appBuilder,
		IApplicationDbContext context,
		IAppSettingsService appSettingsService)
	{
		// wywołując metodę asynchroniczną w metodzie, która nie jest asynchroniczna, trzeba dodać .GetAwaiter().GetResult()

		// na starcie aplikacji wywołana metoda Update i uzupełnienie słownika z ustawieniami
		appSettingsService.Update(context).GetAwaiter().GetResult();

		return appBuilder;
	}
}