using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Controllers.Api;

public abstract class BaseApiController : ControllerBase
{
	private ISender _mediator;
	protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();
}