using GymManager.Application;
using GymManager.Application.Common.Interfaces;
using GymManager.Infrastructure;
using GymManager.UI.Extensions;
using GymManager.UI.Middlewares;
using NLog.Web;

//INFO - Program.cs - kod odpowiedzialny za utworzenie, skonfigurowanie i uruchomienie aplikacji

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// INFO - sposoby dodawania (wstrzykiwania implementacji) Dependency Injection, czyli oznaczenie cyklu �ycia
// AddSingleton - jedna instancja tej klasy w ca�ej aplikacji
// AddScoped - jedna instancja tej klasy b�dzie wsp�lna dla ca�ego requesta
// AddTransient - nowa instancja dla ka�dego kontrolera czy ka�dego serwisu, czyli zawsze jest nowa instancja
//builder.Services.AddScoped<IEmail, Email>();

builder.Services.AddControllersWithViews();

// INFO - dodanie w�asnych serwis�w z innych projekt�w
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// INFO - jedna aplikacja (wiele r�nych szablon�w) dla wielu klient�w
// Konfiguracja silnika Razor jak i gdzie szuka� widok�w dla poszczeg�lnych klient�w
builder.Services.DefineViewLocation(builder.Configuration);

// INFO - Globalizacja - wiele wersji j�zykowych
builder.Services.AddCulture();

// INFO - dodanie NLog
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Information);
builder.Logging.AddNLogWeb();

var app = builder.Build();

// INFO - w tym miejscu trzeba doda� UseInfrastructure, �eby wykonywa� dzia�ania podczas uruchamiania aplikacji
// jako parametry metody b�d� u�yte wstrzykni�te implementacje
using (var scope = app.Services.CreateScope())
{
	// wstrzykiwanie odpowiednich serwis�w
	app.UseInfrastructure(
		scope.ServiceProvider.GetRequiredService<IApplicationDbContext>(),
		app.Services.GetService<IAppSettingsService>()
		);
}

// INFO - ustawienia zale�ne od warto�ci klucza ASPNETCORE_ENVIRONMENT - czyli prze��czanie mi�dzy trybem produkcyjnym a deweloperskim
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

// INFO - testowe zalogowanie kt�ry tryb aplikacji jest ustawiony - produkcja/dev
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
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// INFO - dodanie w�asnego middleware do globalnej obs�ugi wyj�tk�w
app.UseMiddleware<ExceptionHandlerMiddleware>();

// INFO - routing
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();