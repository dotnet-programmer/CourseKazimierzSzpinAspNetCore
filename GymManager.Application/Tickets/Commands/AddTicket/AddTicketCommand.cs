using MediatR;

namespace GymManager.Application.Tickets.Commands.AddTicket;

// W przypadku komend najczęściej brak typu zwracanego bo nic się nie zwraca, jeśli już to np. Id dodanego obiektu
public class AddTicketCommand : IRequest
{
	public string Name { get; set; }
}