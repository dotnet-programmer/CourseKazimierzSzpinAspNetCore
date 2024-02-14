using GymManager.Application;
using GymManager.Infrastructure;
using GymManager.UI.Extensions;
using GymManager.UI.Middlewares;
using NLog.Web;

//INFO - Program.cs - kod odpowiedzialny za utworzenie, skonfigurowanie i uruchomienie aplikacji

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// INFO - dodanie w³asnych serwisów z innych projektów
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

// INFO - sposoby dodawania (wstrzykiwania implementacji) Dependency Injection, czyli oznaczenie cyklu ¿ycia
// AddSingleton - jedna instancja tej klasy w ca³ej aplikacji
// AddScoped - jedna instancja tej klasy bêdzie wspólna dla ca³ego requesta
// AddTransient - nowa instancja dla ka¿dego kontrolera czy ka¿dego serwisu, czyli zawsze jest nowa instancja
//builder.Services.AddScoped<IEmail, Email>();

// INFO - dodanie NLog
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Information);
builder.Logging.AddNLogWeb();

// INFO - jedna aplikacja (wiele ró¿nych szablonów) dla wielu klientów
// Konfiguracja silnika Razor jak i gdzie szukaæ widoków dla poszczególnych klientów
builder.Services.DefineViewLocation(builder.Configuration);

var app = builder.Build();

// INFO - w tym miejscu trzeba dodaæ UseInfrastructure
app.UseInfrastructure();

// INFO - ustawienia zale¿ne od wartoœci klucza ASPNETCORE_ENVIRONMENT - czyli prze³¹czanie miêdzy trybem produkcyjnym a deweloperskim
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
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// INFO - dodanie w³asnego middleware do globalnej obs³ugi wyj¹tków
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();