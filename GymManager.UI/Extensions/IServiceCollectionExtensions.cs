using System.Globalization;
using Microsoft.AspNetCore.Mvc.Razor;

namespace GymManager.UI.Extensions;

public static class IServiceCollectionExtensions
{
	// INFO - jedna aplikacja (wiele różnych szablonów) dla wielu klientów
	// Konfiguracja silnika Razor jak i gdzie szukać widoków dla poszczególnych klientów
	public static void DefineViewLocation(this IServiceCollection services, IConfiguration configuration)
	{
		// pobranie wartości klucza TemplateKey z configa
		string templateKey = configuration.GetSection("TemplateKey").Value;

		// deklaracja jak ma działać silnik Razor
		services.Configure<RazorViewEngineOptions>(x =>
			{
				// wyczyszczenie obecnego formatu
				x.ViewLocationFormats.Clear();

				// dodanie kolejnych miejsc, które będą sprawdzane podczas szukania odpowiedniego widoku
				// najpierw dodanie folderów odpowiednich dla klienta, jeżeli jakis istnieje
				if (templateKey != "Basic")
				{
					// wskazanie głównych widoków (tych ładowanych w pierwszej kolejności)
					// {1} - nazwa kontrolera, {0} - nazwa akcji
					x.ViewLocationFormats.Add("/Views/" + templateKey + "/{1}/{0}" + RazorViewEngine.ViewExtension);
					x.ViewLocationFormats.Add("/Views/" + templateKey + "/Shared/{0}" + RazorViewEngine.ViewExtension);
				}
				// później dodanie ścieżek uniwersalnych - dla wszystkich
				x.ViewLocationFormats.Add("/Views/Basic/{1}/{0}" + RazorViewEngine.ViewExtension);
				x.ViewLocationFormats.Add("/Views/Basic/Shared/{0}" + RazorViewEngine.ViewExtension);
			});
	}

	// INFO - Globalizacja - wiele wersji językowych
	public static void AddCulture(this IServiceCollection services)
	{
		// lista przechowująca informacje o językach, które są wspierane
		var supportedCultures = new List<CultureInfo> { new("pl"), new("en") };

		CultureInfo.DefaultThreadCurrentCulture = supportedCultures[0];
		CultureInfo.DefaultThreadCurrentUICulture = supportedCultures[0];

		services.Configure<RequestLocalizationOptions>(options =>
			{
				options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(supportedCultures[0]);
				options.SupportedCultures = supportedCultures;
				options.SupportedUICultures = supportedCultures;
			});
	}
}