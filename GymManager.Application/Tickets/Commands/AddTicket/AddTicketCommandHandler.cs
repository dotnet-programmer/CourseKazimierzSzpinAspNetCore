using MediatR;

// NuGet Package: Microsoft.Extensions.Logging.Abstractions
using Microsoft.Extensions.Logging;

namespace GymManager.Application.Tickets.Commands.AddTicket;

public class AddTicketCommandHandler : IRequestHandler<AddTicketCommand>
{
	private readonly ILogger _logger;

	public AddTicketCommandHandler(ILogger<AddTicketCommandHandler> logger)
    {
		_logger = logger;
	}

	public async Task Handle(AddTicketCommand request, CancellationToken cancellationToken)
	{
		// tutaj można np. dodać do bazy nowy obiekt
		//var ticket = new Ticket();
		//ticket.Name = request.Name;
		// zapis w bd
		// jeśli nic się nie zwraca, to zwraca się Unit.Value
		//return Unit.Value;

		_logger.LogInformation("Log from AddTicketCommandHandler");

		await Task.CompletedTask;
	}
}