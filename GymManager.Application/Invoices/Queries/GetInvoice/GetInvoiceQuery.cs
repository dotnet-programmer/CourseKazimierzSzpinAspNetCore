using MediatR;

namespace GymManager.Application.Invoices.Queries.GetInvoice;

public class GetInvoiceQuery : IRequest<InvoiceDto>
{
	public int Id { get; set; }
	public string UserId { get; set; }
}