using GymManager.Application;
using GymManager.Application.Common.Interfaces;
using GymManager.Infrastructure;
using GymManager.WebApi.Extensions;
using GymManager.WebApi.Middlewares;
using Microsoft.Extensions.Options;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// wersjonowanie - zmiana tego wpisu potrzebna do poprawnego dzia³ania wersjonowania
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerBearerAuthorization();

// wersjonowanie API - umo¿liwia wydawanie kolejnych wersji bez utraty dzia³ania poprzednich wersji
builder.Services.AddApiVersioning(options =>
{
	options.AssumeDefaultVersionWhenUnspecified = true;
	options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
});

// CORS - mechanizm umo¿liwiaj¹cy wspó³dzielenie zasobów miêdzy serwerami znajduj¹cymi siê w ró¿nych domenach
builder.Services.AddCors();

// dodanie Dependency Injection z innych projektów
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// dodanie NLog 
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Information);
builder.Logging.AddNLogWeb();

// dodanie globalizacji
builder.Services.AddCulture();

var app = builder.Build();

// dodanie Dependency Injection z innych projektów
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
		app.Services.GetService<IWebHostEnvironment>());
}

// dodanie middleware - globalna obs³uga wyj¹tków
app.UseMiddleware<ExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();

	// wersjonowanie API - umo¿liwia wydawanie kolejnych wersji bez utraty dzia³ania poprzednich wersji
	app.UseSwaggerUI(options =>
	{
		// wskazanie adresu dla ka¿dej wersji
		options.SwaggerEndpoint($"/swagger/v1/swagger.json", "v1");
		options.SwaggerEndpoint($"/swagger/v2/swagger.json", "v2");
	});
}

// CORS - mechanizm umo¿liwiaj¹cy wspó³dzielenie zasobów miêdzy serwerami znajduj¹cymi siê w ró¿nych domenach
app.UseCors(x => x
	// zezwalaj na zapytania z dowolnej domeny
	.AllowAnyOrigin()
	// zezwalaj na wywo³anie dowolnej metody
	.AllowAnyMethod()
	// zezwalaj na wywo³anie dowolnego headera
	.AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
