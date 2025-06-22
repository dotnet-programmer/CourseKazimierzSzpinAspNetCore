using GymManager.Application.Common.Interfaces;
using GymManager.Application.Common.Models;
using GymManager.Application.Tickets.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Tickets.Queries.GetPdfTicket;

public class GetPdfTicketQueryHandler(
	IApplicationDbContext context,
	IPdfFileGenerator pdfFileGenerator,
	IQrCodeGenerator qrCodeGenerator
	) : IRequestHandler<GetPdfTicketQuery, TicketPdfVm>
{
	public async Task<TicketPdfVm> Handle(GetPdfTicketQuery request, CancellationToken cancellationToken)
	{
		TicketPdfVm vm = new()
		{
			Handle = Guid.NewGuid().ToString()
		};

		// pobranie informacji o karnecie, dla którego będzie generowany pdf
		var ticket = (await context.Tickets
			.AsNoTracking()
			.Include(x => x.User)
			.FirstOrDefaultAsync(x => x.TicketId == request.TicketId && x.UserId == request.UserId, cancellationToken))
			.ToPrintTicketDto();

		if (ticket == null)
		{
			return null;
		}

		// wygenerowanie kodu QR dla karnetu
		ticket.QrCodeId = qrCodeGenerator.Get(request.TicketId);

		// generowanie pliku PDF z danymi o karnecie
		vm.PdfContent = await pdfFileGenerator.GetAsync(new FileGeneratorParams { Context = request.Context, Model = ticket, ViewTemplate = "TicketPreview" });

		// ustawienie nazwy pliku, który będzie pobrany przez użytkownika
		vm.FileName = $"Karnet_{ticket.Id}.pdf"; 

		return vm;
	}
}