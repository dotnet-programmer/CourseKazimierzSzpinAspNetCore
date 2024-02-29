using GymManager.Application;
using GymManager.Application.Common.Interfaces;
using GymManager.Infrastructure;
using GymManager.WebApi.Extensions;
using Microsoft.Extensions.Options;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS - mechanizm umo�liwiaj�cy wsp�dzielenie zasob�w mi�dzy serwerami znajduj�cymi si� w r�nych domenach
builder.Services.AddCors();

// dodanie Dependency Injection z innych projekt�w
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// dodanie NLog 
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Information);
builder.Logging.AddNLogWeb();

// dodanie globalizacji
builder.Services.AddCulture();

var app = builder.Build();

// dodanie Dependency Injection z innych projekt�w
using (var scope = app.Services.CreateScope())
{
	// dodanie globalizacji
	app.UseRequestLocalization(
		app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

	// wstrzykiwanie odpowiednich serwis�w
	app.UseInfrastructure(
		scope.ServiceProvider.GetRequiredService<IApplicationDbContext>(),
		app.Services.GetService<IAppSettingsService>(),
		app.Services.GetService<IEmailService>(),
		app.Services.GetService<IWebHostEnvironment>());
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// CORS - mechanizm umo�liwiaj�cy wsp�dzielenie zasob�w mi�dzy serwerami znajduj�cymi si� w r�nych domenach
app.UseCors(x => x
	// zezwalaj na zapytania z dowolnej domeny
	.AllowAnyOrigin()
	// zezwalaj na wywo�anie dowolnej metody
	.AllowAnyMethod()
	// zezwalaj na wywo�anie dowolnego headera
	.AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
