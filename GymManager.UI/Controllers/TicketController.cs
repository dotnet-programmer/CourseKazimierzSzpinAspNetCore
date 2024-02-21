using GymManager.Application.Clients.Queries.GetClient;
using GymManager.Application.Tickets.Commands.AddTicket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Controllers;

[Authorize]
public class TicketController : BaseController
{
	public async Task<IActionResult> Tickets()
	{
		var isUserDataCompleted = !string.IsNullOrWhiteSpace((await Mediator.Send(new GetClientQuery { UserId = UserId })).FirstName);

		return View(isUserDataCompleted);
	}
}