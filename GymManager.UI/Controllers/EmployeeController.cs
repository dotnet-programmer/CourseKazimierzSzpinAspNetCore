using GymManager.Application.Common.Interfaces;
using GymManager.Application.Dictionaries;
using GymManager.Application.Employees.Commands.AddEmployee;
using GymManager.Application.Employees.Queries.GetEmployeeBasics;
using GymManager.Infrastructure.Services;
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
}