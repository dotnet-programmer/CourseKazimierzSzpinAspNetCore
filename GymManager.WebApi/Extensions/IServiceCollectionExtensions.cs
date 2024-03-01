using System.Globalization;
using GymManager.WebApi.Models;
using Microsoft.OpenApi.Models;

namespace GymManager.WebApi.Extensions;

public static class IServiceCollectionExtensions
{
	public static void AddCulture(this IServiceCollection service)
	{
		var supportedCultures = new List<CultureInfo>
		{
			new("pl"),
			new("en")
		};

		CultureInfo.DefaultThreadCurrentCulture = supportedCultures[0];
		CultureInfo.DefaultThreadCurrentUICulture = supportedCultures[0];

		service.Configure<RequestLocalizationOptions>(options =>
		{
			options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(supportedCultures[0]);
			options.SupportedCultures = supportedCultures;
			options.SupportedUICultures = supportedCultures;
		});
	}

	#region wersjonowanie API 
	// umożliwia wydawanie kolejnych wersji bez utraty działania poprzednich wersji

	// wersjonowanie dokumentacji, żeby swagger poprawnie działał
	public static void AddSwaggerBearerAuthorization(this IServiceCollection service)
	{
		service.AddSwaggerGen(swagger =>
		{
			// wskazanie kolejnych wersji dokumentacji
			swagger.SwaggerDoc("v1", new OpenApiInfo
			{
				Version = "v1",
				Title = "ASP.NET 6 GymManager Web API 1"
			});
			swagger.SwaggerDoc("v2", new OpenApiInfo
			{
				Version = "v2",
				Title = "ASP.NET 6 GymManager Web API 2"
			});

			// jeśli będą konflikty, to ustaw pierwszą akcję
			swagger.ResolveConflictingActions(x => x.First());
			swagger.OperationFilter<RemoveVersionFromParameter>();
			swagger.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();
		});
	}

	// implementacja uwierzytelnienia

	#endregion
}