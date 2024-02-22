using GymManager.Application.Common.Interfaces;
using GymManager.Application.Tickets.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Tickets.Queries.GetPrintTicket;

public class GetPrintTicketQueryHandler : IRequestHandler<GetPrintTicketQuery, PrintTicketDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IQrCodeGenerator _qrCodeGenerator;

	public GetPrintTicketQueryHandler(IApplicationDbContext context, IQrCodeGenerator qrCodeGenerator)
	{
		_context = context;
		_qrCodeGenerator = qrCodeGenerator;
	}

	public async Task<PrintTicketDto> Handle(GetPrintTicketQuery request, CancellationToken cancellationToken)
	{
		var ticket = (await _context.Tickets
			.AsNoTracking()
			.Include(x => x.User)
			.FirstOrDefaultAsync(x => x.TicketId == request.TicketId && x.UserId == request.UserId))
			.ToPrintTicketDto();

		if (ticket != null)
		{
			ticket.QrCodeId = _qrCodeGenerator.Get(request.TicketId);
		}

		return ticket;
	}
}