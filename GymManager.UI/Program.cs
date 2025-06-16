using AspNetCore.ReCaptcha;
using DataTables.AspNet.AspNetCore;
using GymManager.Application;
using GymManager.Application.Common.Interfaces;
using GymManager.Infrastructure;
using GymManager.Infrastructure.SignalR.UserNotification;
using GymManager.UI.Extensions;
using GymManager.UI.Middlewares;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using NLog.Web;

//Program.cs - kod odpowiedzialny za utworzenie, skonfigurowanie i uruchomienie aplikacji

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// sposoby dodawania (wstrzykiwania implementacji) Dependency Injection, czyli oznaczenie cyklu życia
// AddSingleton - jedna instancja tej klasy w całej aplikacji
// AddScoped - jedna instancja tej klasy będzie wspólna dla całego requesta
// AddTransient - nowa instancja dla każdego kontrolera czy każdego serwisu, czyli zawsze jest nowa instancja
// tutaj przykład wstrzyknięcia klasy Email, wszędzie tam gdzie będzie używany interfejs IEmail
//builder.Services.AddScoped<IEmail, Email>();

builder.Services
	.AddControllersWithViews()
	// żeby przechowywać w TempData duże pliki
	.AddSessionStateTempDataProvider()
	// Globalizacja - wiele wersji językowych
	.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
	// Globalizacja - wiele wersji językowych - dołączenie innych projektów
	.AddDataAnnotationsLocalization(x =>
	{
		x.DataAnnotationLocalizerProvider = (type, factory) =>
		{
			return factory.Create(typeof(GymManager.Application.CommonResources));
		};
	});

// żeby przechowywać w TempData duże pliki
builder.Services.AddSession();

// dodanie własnych serwisów z innych projektów
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// paginacja po stronie serwera
builder.Services.RegisterDataTables();

// jedna aplikacja (wiele różnych szablonów) dla wielu klientów
// Konfiguracja silnika Razor jak i gdzie szukać widoków dla poszczególnych klientów
builder.Services.DefineViewLocation(builder.Configuration);

// Globalizacja - wiele wersji językowych
builder.Services.AddCulture();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// dodanie NLog
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Information);
builder.Logging.AddNLogWeb();

// ReCaptcha
builder.Services.AddReCaptcha(builder.Configuration.GetSection("ReCaptcha"));

var app = builder.Build();

// żeby przechowywać w TempData duże pliki
app.UseSession();

// w tym miejscu trzeba dodać UseInfrastructure, żeby wykonywać działania podczas uruchamiania aplikacji
// jako parametry metody będą użyte wstrzyknięte implementacje
using (var scope = app.Services.CreateScope())
{
	// dodanie globalizacji
	app.UseRequestLocalization(
		app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

	// wstrzykiwanie odpowiednich serwisów
	app.UseInfrastructure(
		scope.ServiceProvider.GetRequiredService<IApplicationDbContext>(),
		app.Services.GetService<IAppSettingsService>(),
		app.Services.GetService<IEmailService>(),
		app.Services.GetService<IWebHostEnvironment>()
		);
}

// ustawienia zależne od wartości klucza ASPNETCORE_ENVIRONMENT - czyli przełączanie między trybem produkcyjnym a deweloperskim
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

// testowe zalogowanie który tryb aplikacji jest ustawiony - produkcja/dev
// pobranie serwisu ILogger<Program> z kontenera Dependency Injection
var logger = app.Services.GetService<ILogger<Program>>();
if (app.Environment.IsDevelopment())
{
	logger.LogInformation("Development mode");
}
else
{
	logger.LogInformation("Production mode");
}

// middleware
// Redirects HTTP requests to HTTPS.
app.UseHttpsRedirection();
// Enables static files, such as HTML, CSS, images, and JavaScript to be served.
app.UseStaticFiles();
// Adds route matching to the middleware pipeline.
app.UseRouting();
// Authorizes a user to access secure resources. This app doesn't use authorization, therefore this line could be removed.
app.UseAuthorization();
// dodanie własnego middleware do globalnej obsługi wyjątków
app.UseMiddleware<ExceptionHandlerMiddleware>();

// routing
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

// dodanie Identity poprzez Scaffolding - potrzebne �eby widoki zosta�y wczytane po wpisaniu adresu URL
// Configures endpoint routing for Razor Pages.
app.MapRazorPages();

// SignalR - serwis do pobierania informacji o aktualnych użytkownikach i ich połączeniach
// parametr /NotificationUserHub to adres URL, w którym jest nazwa klasy która będzie łącznikiem między JavaScript a C#
app.MapHub<NotificationUserHub>("/NotificationUserHub");

app.Run();