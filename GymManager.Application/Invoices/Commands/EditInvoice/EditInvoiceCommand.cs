using MediatR;

namespace GymManager.Application.Invoices.Commands.EditInvoice;

public class EditInvoiceCommand : IRequest
{
	public int Id { get; set; }
	public decimal Value { get; set; }
	public string UserId { get; set; }
}