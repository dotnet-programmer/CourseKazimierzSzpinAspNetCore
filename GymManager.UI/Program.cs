using GymManager.Application;
using GymManager.Infrastructure;
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

var app = builder.Build();

// INFO - w tym miejscu trzeba dodaæ UseInfrastructure
app.UseInfrastructure();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();