using MediatR;

namespace GymManager.Application.Invoices.Commands.EditInvoice;

public class EditInvoiceCommand : IRequest
{
	// Id faktury która będzie aktualizowana
	public int Id { get; set; }
	public decimal Value { get; set; }
	public string UserId { get; set; }
}