using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public abstract class BaseApiController : ControllerBase
{
	private ISender _mediator;
	protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();

	protected string UserId => "e6981a91-a5af-4538-bbb7-0f051b1cd924";
}