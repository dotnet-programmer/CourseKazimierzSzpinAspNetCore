using GymManager.Application.Common.Interfaces;
using GymManager.Application.Common.Models;
using GymManager.Application.Tickets.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Tickets.Queries.GetPdfTicket;
public class GetPdfTicketQueryHandler : IRequestHandler<GetPdfTicketQuery, TicketPdfVm>
{
	private readonly IApplicationDbContext _context;
	private readonly IPdfFileGenerator _pdfFileGenerator;
	private readonly IQrCodeGenerator _qrCodeGenerator;

	public GetPdfTicketQueryHandler(
		IApplicationDbContext context,
		IPdfFileGenerator pdfFileGenerator,
		IQrCodeGenerator qrCodeGenerator)
	{
		_context = context;
		_pdfFileGenerator = pdfFileGenerator;
		_qrCodeGenerator = qrCodeGenerator;
	}

	public async Task<TicketPdfVm> Handle(GetPdfTicketQuery request, CancellationToken cancellationToken)
	{
		TicketPdfVm vm = new()
		{
			Handle = Guid.NewGuid().ToString()
		};

		var ticket = (await _context.Tickets
			.AsNoTracking()
			.Include(x => x.User)
			.FirstOrDefaultAsync(x => x.TicketId == request.TicketId && x.UserId == request.UserId))
			.ToPrintTicketDto();

		if (ticket == null)
		{
			return null;
		}

		ticket.QrCodeId = _qrCodeGenerator.Get(request.TicketId);
		vm.PdfContent = await _pdfFileGenerator.GetAsync(new FileGeneratorParams { Context = request.Context, Model = ticket, ViewTemplate = "TicketPreview" });
		vm.FileName = $"Karnet_{ticket.Id}.pdf";
		return vm;
	}
}