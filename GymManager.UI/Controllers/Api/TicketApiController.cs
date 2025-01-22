using GymManager.Application.Tickets.Commands.MarkTicketAsPaidCommand;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Controllers.Api;

// route musi się zgadzać z tym co jest w AddTicketCommandHandler - $"{_httpContext.AppBaseUrl}/api/ticket/updatestatus"
[Route("api/ticket")]
[ApiController]
public class TicketApiController(ILogger<TicketApiController> logger, IWebHostEnvironment webHostEnvironment) : BaseApiController
{
	private readonly ILogger<TicketApiController> _logger = logger;
	private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

	// akcja API na którą system płatności24 wyśle informację o zmianie statusu 
	[Route("updatestatus")]
	[AllowAnonymous]
	public async Task<IActionResult> UpdateStatus(MarkTicketAsPaidCommand command)
	{
		try
		{
			// wskazanie czy środowisko produkcyjne czy developerskie
			command.IsProduction = _webHostEnvironment.IsProduction();
			await Mediator.Send(command);
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, null);
			return BadRequest();
		}

		return NoContent();
	}
}