using GymManager.Application.Common.Interfaces;
using GymManager.Application.Common.Models;
using GymManager.Application.Invoices.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Invoices.Queries.GetPdfInvoice;

public class GetPdfInvoiceQueryHandler(IApplicationDbContext context, IPdfFileGenerator pdfFileGenerator) : IRequestHandler<GetPdfInvoiceQuery, InvoicePdfVm>
{
	public async Task<InvoicePdfVm> Handle(GetPdfInvoiceQuery request, CancellationToken cancellationToken)
	{
		var invoice = (await context.Invoices
			.Include(x => x.Ticket).ThenInclude(x => x.TicketType)
			.Include(x => x.User).ThenInclude(x => x.Address)
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.InvoiceId == request.InvoiceId && x.UserId == request.UserId, cancellationToken))
			.ToDto();

		return invoice == null
			? null
			: new InvoicePdfVm()
			{
				Handle = Guid.NewGuid().ToString(),
				PdfContent = await pdfFileGenerator.GetAsync(new FileGeneratorParams { Context = request.Context, Model = invoice, ViewTemplate = "InvoicePreview" }),
				FileName = $"Faktura_{invoice.Title}.pdf"
			};
	}
}