using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.WebApi.Controllers;

// wersjonowanie - dostosowanie URL do wersjonowania
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public abstract class BaseApiController : ControllerBase
{
	private ISender _mediator;
	protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();

	protected string UserId => "e6981a91-a5af-4538-bbb7-0f051b1cd924";
}