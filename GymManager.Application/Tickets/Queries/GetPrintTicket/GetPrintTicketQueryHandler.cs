using GymManager.Application.Common.Interfaces;
using GymManager.Application.Tickets.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Tickets.Queries.GetPrintTicket;

public class GetPrintTicketQueryHandler(IApplicationDbContext context, IQrCodeGenerator qrCodeGenerator) : IRequestHandler<GetPrintTicketQuery, PrintTicketDto>
{
	public async Task<PrintTicketDto> Handle(GetPrintTicketQuery request, CancellationToken cancellationToken)
	{
		var ticket = (await context.Tickets
			.AsNoTracking()
			.Include(x => x.User)
			.FirstOrDefaultAsync(x => x.TicketId == request.TicketId && x.UserId == request.UserId, cancellationToken))
			.ToPrintTicketDto();

		if (ticket != null)
		{
			ticket.QrCodeId = qrCodeGenerator.Get(request.TicketId);
		}

		return ticket;
	}
}