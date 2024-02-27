using GymManager.Application.Common.Interfaces;
using GymManager.Domain.Entities;
using GymManager.Infrastructure.Identity;
using GymManager.Infrastructure.Payments;
using GymManager.Infrastructure.Pdf;
using GymManager.Infrastructure.Persistence;
using GymManager.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rotativa.AspNetCore;

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

		// INFO - konfiguracja Identity - użytkownicy w aplikacji
		services.AddIdentity<ApplicationUser, IdentityRole>(options =>
		{
			options.SignIn.RequireConfirmedAccount = true;
			options.Password = new PasswordOptions
			{
				RequireDigit = true,
				RequiredLength = 8,
				RequireLowercase = true,
				RequireUppercase = true,
				RequireNonAlphanumeric = true,
			};
		})
			// zarządzanie rolami
			.AddRoleManager<RoleManager<IdentityRole>>()
		.AddEntityFrameworkStores<ApplicationDbContext>()
		// zdefiniowanie wiadomości walidacyjnych
		.AddErrorDescriber<LocalizedIdentityErrorDescriber>()
		.AddDefaultUI()
		.AddDefaultTokenProviders();

		// dependency injection - używaj AppSettingsService wszędzie tam gdzie jest IAppSettingsService,
		// singleton, bo aplikacja będzie pracować tylko na 1 słowniku z ustawieniami
		services.AddSingleton<IAppSettingsService, AppSettingsService>();

		// singleton, bo apliakcja będzie pracować tylko na 1 obiekcie wysyłającym emaile
		services.AddSingleton<IEmailService, EmailService>();

		// INFO - dodanie serwisu z datą
		services.AddScoped<IDateTimeService, DateTimeService>();

		// INFO - Pobieranie informacji o zalogowanym użytkowniku bez zapytań na bazie danych
		services.AddHttpContextAccessor();
		services.AddSingleton<ICurrentUserService, CurrentUserService>();

		// INFO - abstrakcja / nakładka na RoleManager z Identity
		services.AddScoped<IRoleManagerService, RoleManagerService>();

		// INFO - abstrakcja / nakładka na UserRoleManager z Identity
		services.AddScoped<IUserRoleManagerService, UserRoleManagerService>();

		// INFO - abstrakcja / nakładka na UserManager z Identity
		services.AddScoped<IUserManagerService, UserManagerService>();

		// - INFO - zewnętrzne płatności Przelewy24, ten zapis powoduje użycie fabryki HttpClient
		services.AddHttpClient<IPrzelewy24, Przelewy24>();
		services.AddSingleton<IHttpContext, MyHttpContext>();

		// INFO - serwis do generowania kodów QR
		services.AddScoped<IQrCodeGenerator, QrCodeGenerator>();

		// INFO - serwis do generowania PDF
		services.AddScoped<IPdfFileGenerator, RotativaPdfGenerator>();

		// INFO - serwis do obsługi plików na serwerze
		services.AddSingleton<IFileManagerService, FileManagerService>();

		// INFO - serwis do generowania losowego koloru 
		services.AddScoped<IRandomService, RandomService>();

		return services;
	}

	// tutaj będą wywoływane metody podczas startu aplikacji
	public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder appBuilder,
		IApplicationDbContext context,
		IAppSettingsService appSettingsService,
		IEmailService emailService,
		IWebHostEnvironment webHostEnvironment)
	{
		// wywołując metodę asynchroniczną w metodzie, która nie jest asynchroniczna, trzeba dodać .GetAwaiter().GetResult()

		// na starcie aplikacji wywołana metoda Update i uzupełnienie słownika z ustawieniami
		appSettingsService.Update(context).GetAwaiter().GetResult();

		// na starcie aplikacji wywołana metoda Update i uzupełnienie danych do wysyłki maili z ustawień
		emailService.Update(appSettingsService).GetAwaiter().GetResult();

		// konfiguracja Rotativy do generowania PDF
		RotativaConfiguration.Setup(webHostEnvironment.WebRootPath, "Rotativa");

		return appBuilder;
	}
}