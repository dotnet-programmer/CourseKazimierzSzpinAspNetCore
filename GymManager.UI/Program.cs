using AspNetCore.ReCaptcha;
using GymManager.Application;
using GymManager.Application.Common.Interfaces;
using GymManager.Infrastructure;
using GymManager.UI.Extensions;
using GymManager.UI.Middlewares;
using NLog.Web;

//INFO - Program.cs - kod odpowiedzialny za utworzenie, skonfigurowanie i uruchomienie aplikacji

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// INFO - sposoby dodawania (wstrzykiwania implementacji) Dependency Injection, czyli oznaczenie cyklu życia
// AddSingleton - jedna instancja tej klasy w całej aplikacji
// AddScoped - jedna instancja tej klasy będzie wspólna dla całego requesta
// AddTransient - nowa instancja dla każdego kontrolera czy każdego serwisu, czyli zawsze jest nowa instancja
//builder.Services.AddScoped<IEmail, Email>();

builder.Services.AddControllersWithViews();

// INFO - dodanie własnych serwisów z innych projektów
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// INFO - jedna aplikacja (wiele różnych szablonów) dla wielu klientów
// Konfiguracja silnika Razor jak i gdzie szukać widoków dla poszczególnych klientów
builder.Services.DefineViewLocation(builder.Configuration);

// INFO - Globalizacja - wiele wersji językowych
builder.Services.AddCulture();

// INFO - dodanie NLog
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Information);
builder.Logging.AddNLogWeb();

// INFO - ReCaptcha
builder.Services.AddReCaptcha(builder.Configuration.GetSection("ReCaptcha"));

var app = builder.Build();

// INFO - w tym miejscu trzeba dodać UseInfrastructure, żeby wykonywać działania podczas uruchamiania aplikacji
// jako parametry metody będą użyte wstrzyknięte implementacje
using (var scope = app.Services.CreateScope())
{
	// wstrzykiwanie odpowiednich serwisów
	app.UseInfrastructure(
		scope.ServiceProvider.GetRequiredService<IApplicationDbContext>(),
		app.Services.GetService<IAppSettingsService>(),
		app.Services.GetService<IEmailService>()
		);
}

// INFO - ustawienia zależne od wartości klucza ASPNETCORE_ENVIRONMENT - czyli przełączanie między trybem produkcyjnym a deweloperskim
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

// INFO - testowe zalogowanie który tryb aplikacji jest ustawiony - produkcja/dev
var logger = app.Services.GetService<ILogger<Program>>();
if (app.Environment.IsDevelopment())
{
	logger.LogInformation("Development mode");
}
else
{
	logger.LogInformation("Production mode");
}

// INFO - middleware
// Redirects HTTP requests to HTTPS.
app.UseHttpsRedirection();
// Enables static files, such as HTML, CSS, images, and JavaScript to be served.
app.UseStaticFiles();
// Adds route matching to the middleware pipeline.
app.UseRouting();
// Authorizes a user to access secure resources. This app doesn't use authorization, therefore this line could be removed.
app.UseAuthorization();

// INFO - dodanie własnego middleware do globalnej obsługi wyjątków
app.UseMiddleware<ExceptionHandlerMiddleware>();

// INFO - routing
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

// INFO - dodanie Identity poprzez Scaffolding - potrzebne �eby widoki zosta�y wczytane po wpisaniu adresu URL
// Configures endpoint routing for Razor Pages.
app.MapRazorPages();

app.Run();