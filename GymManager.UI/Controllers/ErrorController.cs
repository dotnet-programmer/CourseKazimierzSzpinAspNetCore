using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Controllers;
public class ErrorController : Controller
{
	// INFO - ustawienie rutingu, czyli odpowiednią ścieżkę
	// czyli ten widok będzie się pojawiał po przekierowaniu do podstrony Error
	[HttpGet("/Error")]
	public IActionResult Index() => View("Error");
}
