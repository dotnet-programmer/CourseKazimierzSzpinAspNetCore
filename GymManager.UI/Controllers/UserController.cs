using GymManager.Application.Dictionaries;
using GymManager.Application.Users.Commands.DeleteUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Controllers;

[Authorize]
public class UserController(ILogger<UserController> logger) : BaseController
{
	private readonly ILogger<UserController> _logger = logger;

	[HttpPost]
	[Authorize(Roles = $"{RolesDict.Administrator},{RolesDict.Employee}")]
	public async Task<IActionResult> DeleteUser(string id)
	{
		try
		{
			await Mediator.Send(new DeleteUserCommand { Id = id });
			return Json(new { success = true });
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, null);
			return Json(new { success = false });
		}
	}
}