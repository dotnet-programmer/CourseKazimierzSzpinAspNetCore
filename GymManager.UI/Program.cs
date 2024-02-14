using GymManager.Application;
using GymManager.Infrastructure;

//INFO - kod odpowiedzialny za utworzenie i uruchomienie aplikacji
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// INFO - dodanie w³asnych serwisów z innych projektów
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

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