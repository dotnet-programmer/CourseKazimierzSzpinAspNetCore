using GymManager.Application.Contacts.Commands.SendContactEmail;
using GymManager.Application.Roles.Commands.AddRole;
using GymManager.Application.Roles.Commands.EditRole;
using GymManager.Application.Roles.Queries.GetEditRole;
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
	public async Task<IActionResult> AddRole(AddRoleCommand command)
	{
		//var result = await MediatorSendValidate(command);

		//if (!result.IsValid)
		//	return View(command);

		bool isValid = false;
		try
		{
			if (ModelState.IsValid)
			{
				await Mediator.Send(command);
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
			return View(command);
		}

		// używane do wyskakujących powiadomień z Toastr
		TempData["Success"] = "Role zostały zaktualizowane";

		return RedirectToAction("Roles");
	}

	public async Task<IActionResult> EditRole(string id) =>
		// pobranie roli o podanym Id z bazy danych
		View(await Mediator.Send(new GetEditRoleQuery { Id = id }));

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> EditRole(EditRoleCommand command)
	{
		//var result = await MediatorSendValidate(command);

		//if (!result.IsValid)
		//	return View(command);

		bool isValid = false;
		try
		{
			if (ModelState.IsValid)
			{
				await Mediator.Send(command);
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
			return View(command);
		}

		TempData["Success"] = "Role zostały zaktualizowane";

		return RedirectToAction("Roles");
	}
}