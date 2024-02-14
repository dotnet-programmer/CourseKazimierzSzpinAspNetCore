using System.Diagnostics;
using GymManager.Application.Tickets.Commands.AddTicket;
using GymManager.Application.Tickets.Queries.GetTicketById;
using GymManager.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Controllers;
public class HomeController : BaseController
{
	//private readonly ILogger<HomeController> _logger;

	//public HomeController(ILogger<HomeController> logger)
	//{
	//	_logger = logger;
	//}

	public async Task<IActionResult> Index()
	{
		// INFO - wywo³anie kwerendy
		// parametr to nazwa kwerendy, czyli klasa Query
		var ticket = await Mediator.Send(new GetTicketByIdQuery { TicketId = 1 });

		// INFO - wywo³anie kwerendy
		await Mediator.Send(new AddTicketCommand { Name = "Ticket1" });

		return View();
	}

	public IActionResult Privacy()
	{
		return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}