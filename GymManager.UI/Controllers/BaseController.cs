using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Controllers;

public abstract class BaseController : Controller
{
	private ISender _mediator;

	// INFO - pole Mediator w bazowym kontrolerze
	// na tym polu są bezpośrednio wywoływane kwerendy i komendy w kontrolerach dziedziczących po tej klasie
	protected ISender Mediator =>
		//since C# 8.0 the ??= null coalescing assignment operator: some_Value ??= some_Value2;
		//Which is a more concise version of: some_Value = some_Value ?? some_Value2;
		_mediator ??= HttpContext.RequestServices.GetService<ISender>();
}