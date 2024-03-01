using MediatR;

namespace GymManager.Application.GymInvoices.Queries.GetPdfGymInvoice;

public class GetPdfGymInvoiceQuery : IRequest<InvoicePdfVm>
{
	public int Id { get; set; }
}