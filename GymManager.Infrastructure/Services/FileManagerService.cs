using GymManager.Application.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace GymManager.Infrastructure.Services;

public class FileManagerService(IWebHostEnvironment webHostEnvironment) : IFileManagerService
{
	public async Task Upload(IEnumerable<IFormFile> files)
	{
		// folder w którym będą trzymane pliki
		// folder wwwroot - _webHostEnvironment.WebRootPath
		// docelowa ścieżka - wwwroot/Content/Files
		var folderRoot = Path.Combine(webHostEnvironment.WebRootPath, "Content", "Files");

		if (!Directory.Exists(folderRoot))
		{
			Directory.CreateDirectory(folderRoot);
		}

		if (files == null || !files.Any())
		{
			return;
		}

		foreach (var file in files)
		{
			if (file.Length > 0)
			{
				var filePath = Path.Combine(folderRoot, file.FileName);

				using (FileStream stream = new(filePath, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}
			}
		}
	}

	public void Delete(string name)
		=> File.Delete(Path.Combine(webHostEnvironment.WebRootPath, "Content", "Files", name));
}