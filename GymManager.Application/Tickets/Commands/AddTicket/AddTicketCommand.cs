using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MediatR;

namespace GymManager.Application.Tickets.Commands.AddTicket;

// W przypadku komend najczęściej brak typu zwracanego bo nic się nie zwraca, jeśli już to np. Id dodanego obiektu
// tutaj typ zwracany TResponse ustawione na string, bo będzie zwracane Id karnetu
public class AddTicketCommand : IRequest<string>
{
	[Required(ErrorMessage = "Pole 'Początek' jest wymagane")]
	[DisplayName("Początek")]
	public DateTime StartDate { get; set; }

	[DisplayName("Cena")]
	public decimal Price { get; set; }

	[DisplayName("Typ karnetu")]
	public int TicketTypeId { get; set; }

	public string UserId { get; set; }
}