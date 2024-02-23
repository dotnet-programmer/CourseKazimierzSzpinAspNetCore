using GymManager.Application.Dictionaries;
using GymManager.Application.Files.Commands.UploadFile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Controllers;

[Authorize(Roles = RolesDict.Administrator)]
public class FileController : BaseController
{
	public async Task<IActionResult> Files() => 
		View();

	[HttpPost]
	public async Task<IActionResult> Upload(IEnumerable<IFormFile> files)
	{
		try
		{
			await Mediator.Send(new UploadFileCommand { Files = files });

			return Json(new { success = true });
		}
		catch (Exception)
		{
			return Json(new { success = false });
		}
	}
}