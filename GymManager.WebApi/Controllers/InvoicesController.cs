﻿using GymManager.Application.Invoices.Commands.AddInvoice;
using GymManager.Application.Invoices.Commands.DeleteInvoice;
using GymManager.Application.Invoices.Commands.EditInvoice;
using GymManager.Application.Invoices.Queries.GetInvoice;
using GymManager.Application.Invoices.Queries.GetInvoices;
using GymManager.Application.Invoices.Queries.GetPdfInvoice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.WebApi.Controllers;

// wersjonowanie - określenie ogólnej wersji API dla całego kontrolera
[ApiVersion("1")]
[ApiExplorerSettings(GroupName = "v1")]
// atrybut Authorize potrzebny przy używaniu JWT
[Authorize]
public class InvoicesController : BaseApiController
{
	// pobranie wszystkich faktur
	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var invoices = await Mediator.Send(new GetInvoicesQuery { UserId = UserId });
		return Ok(invoices);
	}

	// wersjonowanie - określenie konkretnej wersji API dla wybranej akcji
	[MapToApiVersion("2.0")]
	[ApiExplorerSettings(GroupName = "v2")]
	[HttpGet]
	public async Task<IActionResult> GetAllv2()
		=> Ok(new List<InvoiceBasicsDto> { new() { Id = 100, CreatedDate = new DateTime(2000, 1, 1), Title = "1", UserId = "1", UserName = "Test", Value = 1 } });

	// pobranie pojedynczej faktury dla podanego użytkownika
	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(int id)
	{
		var invoice = await Mediator.Send(new GetInvoiceQuery
		{
			UserId = UserId,
			Id = id
		});

		return invoice != null ? Ok(invoice) : NotFound();
	}

	// dodawanie nowej faktury
	[HttpPost]
	public async Task<IActionResult> Add(AddInvoiceCommand command)
	{
		command.UserId = UserId;
		return Ok(await Mediator.Send(command));
	}

	// aktualizacja wybranej faktury
	[HttpPut]
	public async Task<IActionResult> Edit(EditInvoiceCommand command)
	{
		command.UserId = UserId;
		await Mediator.Send(command);
		return NoContent();
	}

	// usuwanie wybranej faktury
	[HttpDelete]
	public async Task<IActionResult> Delete(DeleteInvoiceCommand command)
	{
		command.UserId = UserId;
		await Mediator.Send(command);
		return NoContent();
	}

	// ustalenie adresu strony - do głównego adresu trzeba dopisać /pdf/id
	[HttpGet("pdf/{id}")]
	public async Task<IActionResult> GetPdf(int id)
	{
		var vm = await Mediator.Send(new GetPdfInvoiceQuery
		{
			UserId = UserId,
			InvoiceId = id,
			Context = ControllerContext
		});

		return vm != null ? Ok(vm) : NotFound();
	}
}