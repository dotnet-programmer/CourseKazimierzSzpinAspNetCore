using System.Security.Claims;
using GymManager.UI.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Controllers;

// TODO - sprawdzić dla której komendy albo kwerendy działa, może trzeba zrobić inną wersje dla tych niezwracajacych żadnych danych albo popróbować z typem generycznym
public abstract class BaseController : Controller
{

	// INFO - pole Mediator w bazowym kontrolerze
	// na tym polu są bezpośrednio wywoływane kwerendy i komendy w kontrolerach dziedziczących po tej klasie
	private ISender _mediator;
	protected ISender Mediator =>
		//since C# 8.0 the ??= null coalescing assignment operator: some_Value ??= some_Value2;
		//Which is a more concise version of: some_Value = some_Value ?? some_Value2;
		_mediator ??= HttpContext.RequestServices.GetService<ISender>();

	protected string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

	// walidacja danych przed wysłaniem komendy
	protected async Task<MediatorValidateResponse<T>> MediatorSendValidate<T>(IRequest<T> request)
	{
		// na początku budowana jest odpowiedź
		var response = new MediatorValidateResponse<T> { IsValid = false };

		try
		{
			if (ModelState.IsValid)
			{
				response.Model = await Mediator.Send(request);
				response.IsValid = true;
				return response;
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

		return response;
	}
}