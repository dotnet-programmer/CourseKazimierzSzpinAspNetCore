﻿using DataTables.AspNet.Core;
using GymManager.Application.Clients.Queries.GetClient;
using GymManager.Application.Tickets.Queries.GetAddTicket;
using GymManager.Application.Tickets.Queries.GetClientsTickets;
using GymManager.UI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Controllers;

[Authorize]
public class TicketController : BaseController
{
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
}