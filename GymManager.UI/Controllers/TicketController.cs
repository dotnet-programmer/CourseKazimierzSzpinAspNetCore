using DataTables.AspNet.Core;
using GymManager.Application.Clients.Queries.GetClient;
using GymManager.Application.Tickets.Commands.AddTicket;
using GymManager.Application.Tickets.Queries.GetAddTicket;
using GymManager.Application.Tickets.Queries.GetClientsTickets;
using GymManager.Application.Tickets.Queries.GetPrintTicket;
using GymManager.UI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Controllers;

[Authorize]
public class TicketController : BaseController
{
	private readonly IConfiguration _configuration;
	private readonly ILogger _logger;

	public TicketController(IConfiguration configuration, ILogger<TicketController> logger)
	{
		_configuration = configuration;
		_logger = logger;
	}

	public async Task<IActionResult> TicketsAsync()
	{
		bool isUserDataCompleted = !string.IsNullOrWhiteSpace((await Mediator.Send(new GetClientQuery { UserId = UserId })).FirstName);
		return View(isUserDataCompleted);
	}

	// IDataTablesRequest - pakiet NuGet - DataTables.AspNet.Core
	public async Task<IActionResult> TicketsDataTable(IDataTablesRequest request)
	{
		// na podstawie requesta zostaną zwrócone odpowiednie dane
		var tickets = await Mediator.Send(new GetClientsTicketsQuery
		{
			UserId = UserId,
			PageSize = request.Length,
			SearchValue = request.Search.Value,
			PageNumber = request.GetPageNumber(),
			OrderInfo = request.GetOrderInfo()
		});

		return request.GetResponse(tickets);
	}

	public async Task<IActionResult> AddTicket() => 
		View(await Mediator.Send(new GetAddTicketQuery()));

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> AddTicket(AddTicketVm vm)
	{
		var result = await MediatorSendValidate(new AddTicketCommand
			{
				StartDate = vm.Ticket.StartDate,
				TicketTypeId = vm.Ticket.TicketTypeId,
				UserId = UserId,
				Price = vm.Ticket.Price,
			});

		if (!result.IsValid)
		{
			return View(vm);
		}

		TempData["Success"] = "Nowy karnet został utworzony, oczekiwanie na zweryfikowanie płatności.";

		return Redirect($"{_configuration.GetValue<string>("Przelewy24:BaseUrl")}/trnRequest/{result.Model}");
	}

	public async Task<IActionResult> TicketPreview(string id)
	{
		var ticket = await Mediator.Send(new GetPrintTicketQuery
			{
				TicketId = id,
				UserId = UserId
			});

		return View(ticket);
	}
}