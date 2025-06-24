using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GymManager.WebApi.Models;

// wersjonowanie API - umożliwia wydawanie kolejnych wersji bez utraty działania poprzednich wersji

// konfiguracja żeby swagger poprawnie się wyświetlał
public class RemoveVersionFromParameter : IOperationFilter
{
	public void Apply(OpenApiOperation operation, OperationFilterContext context)
	{
		if (operation.Parameters.Count > 0)
		{
			var versionParameter = operation.Parameters.Single(x => x.Name == "version");
			operation.Parameters.Remove(versionParameter);
		}
	}
}

public class ReplaceVersionWithExactValueInPathFilter : IDocumentFilter
{
	public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
	{
		var paths = swaggerDoc.Paths;
		swaggerDoc.Paths = new OpenApiPaths();

		foreach (var path in paths)
		{
			var key = path.Key.Replace("v{version}", swaggerDoc.Info.Version);
			var value = path.Value;
			swaggerDoc.Paths.Add(key, value);
		}
	}
}