using GymManager.Application.Common.Interfaces;
using MediatR;

namespace GymManager.Application.GymInvoices.Queries.GetPdfGymInvoice;

public class GetPdfGymInvoiceQueryHandler(IGymInvoices gymInvoices) : IRequestHandler<GetPdfGymInvoiceQuery, InvoicePdfVm>
{
	public async Task<InvoicePdfVm> Handle(GetPdfGymInvoiceQuery request, CancellationToken cancellationToken) =>
		await gymInvoices.GetPdfInvoice(request.Id);
}