using GymManager.Application.Common.Interfaces;
using GymManager.Application.Dictionaries;
using GymManager.Application.Employees.Queries.GetEmployeeBasics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Controllers;

[Authorize(Roles = RolesDict.Administrator)]
public class EmployeeController : BaseController
{
	public async Task<IActionResult> Employees() =>
		View(await Mediator.Send(new GetEmployeeBasicsQuery()));
}