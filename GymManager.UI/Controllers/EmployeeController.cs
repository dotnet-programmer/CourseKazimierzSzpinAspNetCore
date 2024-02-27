using GymManager.Application.Common.Interfaces;
using GymManager.Application.Dictionaries;
using GymManager.Application.Employees.Commands.AddEmployee;
using GymManager.Application.Employees.Queries.GetEditEmployee;
using GymManager.Application.Employees.Queries.GetEmployeeBasics;
using GymManager.Application.Employees.Queries.GetEmployeePage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Controllers;

[Authorize(Roles = RolesDict.Administrator)]
public class EmployeeController : BaseController
{
	private readonly IDateTimeService _dateTimeService;

	public EmployeeController(IDateTimeService dateTimeService) =>
		_dateTimeService = dateTimeService;

	public async Task<IActionResult> Employees() =>
		View(await Mediator.Send(new GetEmployeeBasicsQuery()));

	public async Task<IActionResult> AddEmployee() =>
		View(new AddEmployeeCommand { DateOfEmployment = _dateTimeService.Now });

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> AddEmployee(AddEmployeeCommand command)
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

		TempData["Success"] = "Pracownik został dodany.";

		return RedirectToAction("Employees");
	}

	public async Task<IActionResult> EditEmployee(string employeeId) =>
		View(await Mediator.Send(new GetEditEmployeeQuery { UserId = employeeId }));

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> EditEmployee(EditEmployeeVm viewModel)
	{
		//var result = await MediatorSendValidate(viewModel.Employee);

		//if (!result.IsValid)
		//	return View(viewModel);

		bool isValid = false;
		try
		{
			if (ModelState.IsValid)
			{
				await Mediator.Send(viewModel.Employee);
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
			return View(viewModel);
		}

		TempData["Success"] = "Dane pracownika zostały zaktualizowane.";

		return RedirectToAction("Employees");
	}

	// wyświetlanie strony profilowej pracownika
	// strona będzie wyświetlała się po dopisaniu do adresu trener/strona_trenera
	[Route("trener/{employeePageUrl}")]
	public async Task<IActionResult> EmployeePage(string employeePageUrl)
	{
		// pobranie info o stronie z bazy danych
		var page = await Mediator.Send(new GetEmployeePageQuery { Url = employeePageUrl });

		// jeśli taka strona istnieje, to zwróć tą stronę, jeśli nie, to NotFound
		return page != null ? View(page) : (IActionResult)NotFound();
	}
}