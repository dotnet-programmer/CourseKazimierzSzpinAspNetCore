using GymManager.Application.Clients.Commands.EditClient;
using GymManager.Application.Clients.Queries.GetClient;
using GymManager.Application.Clients.Queries.GetEditClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Controllers;

[Authorize]
public class ClientController : BaseController
{
	public IActionResult Dashboard() =>
		View();

	public async Task<IActionResult> Client() => 
		View(await Mediator.Send(new GetClientQuery { UserId = UserId }));

	public async Task<IActionResult> EditClient() => 
		View(await Mediator.Send(new GetEditClientQuery { UserId = UserId }));

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> EditClient(EditClientCommand command)
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
		// przesłanie z powrotem wypełnionych pól do formularza, dzięki temu nie będzie musiał wyepłniać całego od nowa
		if (!isValid)
		{
			return View(command);
		}

		TempData["Success"] = "Twoje dane zostały zaktualizowane";

		return RedirectToAction("Client");
	}
}