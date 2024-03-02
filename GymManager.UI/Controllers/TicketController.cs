using DataTables.AspNet.Core;
using GymManager.Application.Clients.Queries.GetClient;
using GymManager.Application.Tickets.Commands.AddTicket;
using GymManager.Application.Tickets.Queries.GetAddTicket;
using GymManager.Application.Tickets.Queries.GetClientsTickets;
using GymManager.Application.Tickets.Queries.GetPdfTicket;
using GymManager.Application.Tickets.Queries.GetPrintTicket;
using GymManager.UI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GymManager.UI.Controllers;

[Authorize]
public class TicketController : BaseController
{
	private readonly IConfiguration _configuration;
	private readonly ILogger _logger;
	private readonly IStringLocalizer<CommonResources> _localizer;

	public TicketController(
		IConfiguration configuration, 
		ILogger<TicketController> logger, 
		IStringLocalizer<CommonResources> localizer)
	{
		_configuration = configuration;
		_logger = logger;
		_localizer = localizer;
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

		TempData["Success"] = _localizer["TicketCreated"].Value;

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

	public async Task<IActionResult> TicketToPdf(string id)
	{
		try
		{
			var ticketPdfVm = await Mediator.Send(new GetPdfTicketQuery
			{
				TicketId = id,
				UserId = UserId,
				Context = ControllerContext
			});

			TempData.Put(ticketPdfVm.Handle, ticketPdfVm.PdfContent);

			return Json(new
			{
				success = true,
				fileGuid = ticketPdfVm.Handle,
				fileName = ticketPdfVm.FileName
			});
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, null);
			return Json(new { success = false });
		}
	}

	public IActionResult DownloadTicketPdf(string fileGuid, string fileName)
	{
		if (TempData[fileGuid] == null)
		{
			throw new Exception("Błąd przy próbie eksportu karnetu do PDF.");
		}

		return File(TempData.Get<byte[]>(fileGuid), "application/pdf", fileName);
	}

	public async Task<IActionResult> PrintTicket(string id)
	{
		var ticket = await Mediator.Send(new GetPrintTicketQuery
		{
			TicketId = id,
			UserId = UserId
		});

		return View("TicketPreview", ticket);
	}
}