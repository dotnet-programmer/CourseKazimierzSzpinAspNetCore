using System.Security.Claims;
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

	// JWT - odczytanie danych z tokena
	protected string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
}