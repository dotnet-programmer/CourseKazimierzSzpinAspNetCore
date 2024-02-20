using GymManager.Application.Contacts.Commands.SendContactEmail;
using GymManager.Application.Roles.Commands.AddRole;
using GymManager.Application.Roles.Queries.GetRoles;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Controllers;

public class RoleController : BaseController
{
	public async Task<IActionResult> Roles() =>
		View(await Mediator.Send(new GetRolesQuery()));

	public IActionResult AddRole() => 
		View(new AddRoleCommand());

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> AddRole(AddRoleCommand addRoleCommand)
	{
		//var result = await MediatorSendValidate(command);

		//if (!result.IsValid)
		//	return View(command);

		bool isValid = false;
		try
		{
			if (ModelState.IsValid)
			{
				await Mediator.Send(addRoleCommand);
				isValid = true;
			}
		}
		catch (Application.Common.Exceptions.ValidationException exception)
		{
			foreach (var item in exception.Errors)
			{
				// przekazanie wszystkich błędów z powrotem do widoku
				ModelState.AddModelError(item.Key, string.Join(". ", item.Value));
			}
		}

		if (!isValid)
		{
			return View(addRoleCommand);
		}

		// używane do wyskakujących powiadomień z Toastr
		TempData["Success"] = "Role zostały zaktualizowane";

		return RedirectToAction("Roles");
	}
}