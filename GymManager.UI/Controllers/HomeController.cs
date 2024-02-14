using System.Diagnostics;
using GymManager.Application.Tickets.Commands.AddTicket;
using GymManager.Application.Tickets.Queries.GetTicketById;
using GymManager.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Controllers;
public class HomeController : BaseController
{
	private readonly ILogger _logger;

	public HomeController(ILogger<HomeController> logger)
	{
		_logger = logger;
	}

	public async Task<IActionResult> Index()
	{
		// INFO - wywo�anie kwerendy
		// parametr to nazwa kwerendy, czyli klasa Query
		var ticket = await Mediator.Send(new GetTicketByIdQuery { TicketId = 1 });

		// INFO - wywo�anie kwerendy
		await Mediator.Send(new AddTicketCommand { Name = "Ticket1" });

		// INFO - u�ycie NLog
		_logger.LogInformation("LogInformation");
		_logger.LogError(new Exception("LogError"), null);

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