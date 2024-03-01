using System.ComponentModel.DataAnnotations;
using MediatR;

namespace GymManager.Application.Invoices.Commands.AddInvoice;

// komenda zwracająca wartość
// zgodnie z konwencją REST-a akcja post która dodaje nowy zasób powinna zwrócić ID tego zasobu
// TResponse - typ int, czyli ID dodanego zasobu
public class AddInvoiceCommand : IRequest<int>
{
	[Required]
	public string TicketId { get; set; }

	public string UserId { get; set; }
}