using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace GymManager.Application.Tickets.Commands.AddTicket;

// W przypadku komend najczęściej brak typu zwracanego bo nic się nie zwraca, jeśli już to np. Id dodanego obiektu
// tutaj typ zwracany TResponse ustawione na string, bo będzie zwracane Id karnetu
public class AddTicketCommand : IRequest<string>
{
	// Globalizacja - tłumaczenia brane są z pliku Resources - CommonResources.resx
	// zamiast tekstu wpisanego na sztywno, używany jest klucz z pliku Resources
	[Required(ErrorMessage = "FieldStartDateTicketIsRequired")]
	[DisplayName("StartDateTicket")]
	public DateTime StartDate { get; set; }

	[DisplayName("Price")]
	public decimal Price { get; set; }

	[DisplayName("TypeOfTicket")]
	public int TicketTypeId { get; set; }

	public string UserId { get; set; }
}