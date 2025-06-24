using Asp.Versioning;
using GymManager.Application;
using GymManager.Application.Common.Interfaces;
using GymManager.Infrastructure;
using GymManager.WebApi.Extensions;
using GymManager.WebApi.Middlewares;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Swagger - Swashbuckle - narzêdzie do generowania dokumentacji API, innym rozwi¹zaniem do testowania API jest np. Insomnia
// zakomentowane, poniewa¿ wersjonowanie API wymaga dodatkowej konfiguracji Swaggera, która znajduje sie w builder.Services.AddSwaggerBearerAuthorization();
//builder.Services.AddSwaggerGen(); 

// Microsoft.AspNetCore.Mvc.Versioning
// wersjonowanie - zmiana tego wpisu potrzebna do poprawnego dzia³ania wersjonowania
// jest to metoda rozszerzaj¹ca IServiceCollection, która dodaje Swaggera i wersjonowanie, zastêpuje metodê builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerBearerAuthorization();
// wersjonowanie API - umo¿liwia wydawanie kolejnych wersji bez utraty dzia³ania poprzednich wersji
builder.Services.AddApiVersioning(options =>
{
	options.ReportApiVersions = true;
	options.DefaultApiVersion = new ApiVersion(1);
	//options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
	options.AssumeDefaultVersionWhenUnspecified = true;
});

// dodanie tokena JWT (JSON Web Token) - mechanizm uwierzytelniania, który jest u¿ywany do zabezpieczania API
builder.Services.AddBearerAuthentication(builder.Configuration);

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
	// Swagger - narzêdzie do generowania dokumentacji API
	app.UseSwagger();
	//app.UseSwaggerUI();

	// Microsoft.AspNetCore.Mvc.Versioning
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

// umo¿liwienie korzystania z plików statycznych (obrazy, exe itp) w WebApi
app.UseFileServer(new FileServerOptions
{
	// wskazanie fizycznej œcie¿ki
	FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
	// ta fizyczna œcie¿ka do folderu bêdzie dostêpna po dopisaniu do adresu URL /wwwroot
	RequestPath = "/wwwroot",
	EnableDefaultFiles = true
});

app.UseHttpsRedirection();

// dodanie JWT - dodaæ UseAuthentication przed UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
