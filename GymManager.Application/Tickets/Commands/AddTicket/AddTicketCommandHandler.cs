using MediatR;

namespace GymManager.Application.Tickets.Commands.AddTicket;

public class AddTicketCommandHandler : IRequestHandler<AddTicketCommand>
{
	public async Task Handle(AddTicketCommand request, CancellationToken cancellationToken) =>

	// tutaj można np. dodać do bazy nowy obiekt
	//var ticket = new Ticket();
	//ticket.Name = request.Name;
	// zapis w bd
	// jeśli nic się nie zwraca, to zwraca się Unit.Value
	//return Unit.Value;

	await Task.CompletedTask;
}