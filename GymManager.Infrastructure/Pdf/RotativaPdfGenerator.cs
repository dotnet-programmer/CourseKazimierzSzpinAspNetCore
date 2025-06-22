using GymManager.Application.Common.Interfaces;
using GymManager.Application.Common.Models;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;

namespace GymManager.Infrastructure.Pdf;

public class RotativaPdfGenerator : IPdfFileGenerator
{
	public async Task<byte[]> GetAsync(FileGeneratorParams fileGeneratorParams)
	{
		// na podstawie przekazanego widoku zostanie wygenerowany pdf
		// ViewTemplate - nazwa widoku
		// Model - model, który ma być na tym widoku
		ViewAsPdf pdfResult = new(fileGeneratorParams.ViewTemplate, fileGeneratorParams.Model)
		{
			// dodatkowe ustawienia pdf-a
			PageSize = Size.A4,
			PageOrientation = Orientation.Portrait
		};

		return await pdfResult.BuildFile(fileGeneratorParams.Context);
	}
}