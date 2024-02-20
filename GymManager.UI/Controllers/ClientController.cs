using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Controllers;

[Authorize]
public class ClientController : BaseController
{
	public IActionResult Dashboard() =>
		View();

	public IActionResult Client() =>
		View();
}