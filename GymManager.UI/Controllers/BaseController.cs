using System.Globalization;
using System.Security.Claims;
using GymManager.UI.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Controllers;

public abstract class BaseController : Controller
{
	// pole Mediator w bazowym kontrolerze
	// na tym polu są bezpośrednio wywoływane kwerendy i komendy w kontrolerach dziedziczących po tej klasie
	private ISender _mediator;

	// Since C# 8.0 the ??= null coalescing assignment operator: some_Value ??= some_Value2;
	// Which is a more concise version of: some_Value = some_Value ?? some_Value2;
	protected ISender Mediator
		=> _mediator ??= HttpContext.RequestServices.GetService<ISender>();

	protected string UserId
		=> User.FindFirstValue(ClaimTypes.NameIdentifier);

	// globalizacja - wersje językowe
	protected string CurrentLanguage
		=> CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

	// walidacja danych przed wysłaniem komendy
	protected async Task<MediatorValidateResponse<TResponse>> MediatorSendValidate<TResponse>(IRequest<TResponse> request)
	{
		// na początku budowana jest odpowiedź
		MediatorValidateResponse<TResponse> response = new() { IsValid = false };

		try
		{
			if (ModelState.IsValid)
			{
				// response.Model będzie zawierało wynik działania komendy, jeżeli ta będzie zawierała dane do zwrócenia
				response.Model = await Mediator.Send(request);
				response.IsValid = true;
				// zwrócenie odpowiedzi z ewentualną zawartością
				return response;
			}
		}
		catch (Application.Common.Exceptions.ValidationException exception)
		{
			// przekazanie wszystkich błędów z powrotem do widoku
			foreach (var item in exception.Errors)
			{
				ModelState.AddModelError(item.Key, string.Join(". ", item.Value));
			}
		}

		// zwrócenie odpowiedzi z ustawieniem IsValid = false
		return response;
	}
}