using Microsoft.AspNetCore.Mvc.Razor;

namespace GymManager.UI.Extensions;

public static class IServiceCollectionExtensions
{
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
}