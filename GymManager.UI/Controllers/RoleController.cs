using GymManager.Application.Common.Exceptions;
using GymManager.Application.Dictionaries;
using GymManager.Application.Roles.Commands.AddRole;
using GymManager.Application.Roles.Commands.DeleteRole;
using GymManager.Application.Roles.Commands.EditRole;
using GymManager.Application.Roles.Queries.GetEditRole;
using GymManager.Application.Roles.Queries.GetRoles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Controllers;

[Authorize(Roles = RolesDict.Administrator)]
public class RoleController(ILogger<RoleController> logger) : BaseController
{
	private readonly ILogger _logger = logger;

	public async Task<IActionResult> Roles() =>
		// poprawienie przepływu aplikacji – MVC jak SPA
		//await Task.Delay(2000);
		//var roles = await Mediator.Send(new GetRolesQuery());
		//return string.IsNullOrWhiteSpace(Request.Headers["X-PJAX"]) 
		//	? View(roles) 
		//	: PartialView(roles);

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

	// pobranie roli o podanym Id z bazy danych
	public async Task<IActionResult> EditRole(string id) =>
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

	[HttpPost]
	public async Task<IActionResult> DeleteRole(string id)
	{
		try
		{
			await Mediator.Send(new DeleteRoleCommand { Id = id });

			return Json(new { success = true });
		}
		catch (ValidationException exception)
		{
			return Json(new { success = false, message = string.Join(". ", exception.Errors.Select(x => string.Join(". ", x.Value.Select(y => y)))) });
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, null);
			return Json(new { success = false });
		}
	}
}