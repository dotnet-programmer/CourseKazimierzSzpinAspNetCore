using GymManager.Application.Common.Interfaces;
using GymManager.Application.Common.Models;
using GymManager.Application.Tickets.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Tickets.Queries.GetPdfTicket;

public class GetPdfTicketQueryHandler(
	IApplicationDbContext context,
	IPdfFileGenerator pdfFileGenerator,
	IQrCodeGenerator qrCodeGenerator) : IRequestHandler<GetPdfTicketQuery, TicketPdfVm>
{
	public async Task<TicketPdfVm> Handle(GetPdfTicketQuery request, CancellationToken cancellationToken)
	{
		TicketPdfVm vm = new()
		{
			Handle = Guid.NewGuid().ToString()
		};

		var ticket = (await context.Tickets
			.AsNoTracking()
			.Include(x => x.User)
			.FirstOrDefaultAsync(x => x.TicketId == request.TicketId && x.UserId == request.UserId, cancellationToken))
			.ToPrintTicketDto();

		if (ticket == null)
		{
			return null;
		}

		ticket.QrCodeId = qrCodeGenerator.Get(request.TicketId);
		vm.PdfContent = await pdfFileGenerator.GetAsync(new FileGeneratorParams { Context = request.Context, Model = ticket, ViewTemplate = "TicketPreview" });
		vm.FileName = $"Karnet_{ticket.Id}.pdf";

		return vm;
	}
}