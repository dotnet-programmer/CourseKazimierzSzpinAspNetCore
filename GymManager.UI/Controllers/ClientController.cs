using GymManager.Application.Clients.Commands.AddClient;
using GymManager.Application.Clients.Commands.EditClient;
using GymManager.Application.Clients.Queries.GetClient;
using GymManager.Application.Clients.Queries.GetClientDashboard;
using GymManager.Application.Clients.Queries.GetClientsBasics;
using GymManager.Application.Clients.Queries.GetEditAdminClient;
using GymManager.Application.Clients.Queries.GetEditClient;
using GymManager.Application.Dictionaries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Controllers;

[Authorize]
public class ClientController : BaseController
{
	public async Task<IActionResult> Dashboard() 
		=> View(await Mediator.Send(new GetClientDashboardQuery { UserId = UserId }));

	public async Task<IActionResult> Client() 
		=> View(await Mediator.Send(new GetClientQuery { UserId = UserId }));

	public async Task<IActionResult> EditClient() 
		=> View(await Mediator.Send(new GetEditClientQuery { UserId = UserId }));

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> EditClient(EditClientCommand command)
	{
		var result = await MediatorSendValidate(command);

		if (!result.IsValid)
		{
			return View(command);
		}

		TempData["Success"] = "Twoje dane zostały zaktualizowane";
		return RedirectToAction("Client");
	}

	[Authorize(Roles = $"{RolesDict.Administrator},{RolesDict.Employee}")]
	public async Task<IActionResult> Clients()
		=> View(await Mediator.Send(new GetClientsBasicsQuery()));

	[Authorize(Roles = $"{RolesDict.Administrator},{RolesDict.Employee}")]
	public IActionResult AddClient()
		=> View(new AddClientCommand());

	[HttpPost]
	[ValidateAntiForgeryToken]
	[Authorize(Roles = $"{RolesDict.Administrator},{RolesDict.Employee}")]
	public async Task<IActionResult> AddClient(AddClientCommand command)
	{
		var result = await MediatorSendValidate(command);

		if (!result.IsValid)
		{
			return View(command);
		}

		TempData["Success"] = "Dane o klientach zostały zaktualizowane";
		return RedirectToAction("Clients");
	}

	[Authorize(Roles = $"{RolesDict.Administrator},{RolesDict.Employee}")]
	public async Task<IActionResult> EditAdminClient(string clientId) 
		=> View(await Mediator.Send(new GetEditAdminClientQuery { UserId = clientId }));

	[HttpPost]
	[ValidateAntiForgeryToken]
	[Authorize(Roles = $"{RolesDict.Administrator},{RolesDict.Employee}")]
	public async Task<IActionResult> EditAdminClient(EditAdminClientVm vm)
	{
		var result = await MediatorSendValidate(vm.Client);

		if (!result.IsValid)
		{
			return View(vm);
		}

		TempData["Success"] = "Dane o klientach zostały zaktualizowane";
		return RedirectToAction("Clients");
	}
}