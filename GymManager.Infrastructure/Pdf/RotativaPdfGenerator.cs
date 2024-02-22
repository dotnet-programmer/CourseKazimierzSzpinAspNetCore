using GymManager.Application.Common.Interfaces;
using GymManager.Application.Common.Models;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;

namespace GymManager.Infrastructure.Pdf;

public class RotativaPdfGenerator : IPdfFileGenerator
{
	public async Task<byte[]> GetAsync(FileGeneratorParams @params)
	{
		ViewAsPdf pdfResult = new(@params.ViewTemplate, @params.Model)
		{
			PageSize = Size.A4,
			PageOrientation = Orientation.Portrait
		};

		return await pdfResult.BuildFile(@params.Context);
	}
}