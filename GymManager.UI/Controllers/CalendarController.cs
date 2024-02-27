using GymManager.Application.Dictionaries;
using GymManager.Application.EmployeeEvents.Queries.GetEmployeeEvents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Controllers;

[Authorize(Roles = $"{RolesDict.Administrator},{RolesDict.Pracownik}")]
public class CalendarController : BaseController
{
	public IActionResult Calendar() =>
		View();

	// pobranie informacji o wszystkich zdarzeniach pracowników, używana w widoku generowania całego kalendarza
	public async Task<IActionResult> GetEmployeeEvents() 
		=> Json(await Mediator.Send(new GetEmployeeEventsQuery()));
}