using System.Globalization;

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
}