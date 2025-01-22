using GymManager.Application.Common.Interfaces;
using GymManager.Application.Invoices.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Invoices.Queries.GetInvoice;

public class GetInvoiceQueryHandler(IApplicationDbContext context) : IRequestHandler<GetInvoiceQuery, InvoiceDto>
{
	public async Task<InvoiceDto> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
		=> (await context.Invoices
			.Include(x => x.Ticket).ThenInclude(x => x.TicketType)
			.Include(x => x.User).ThenInclude(x => x.Address)
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.InvoiceId == request.Id && x.UserId == request.UserId, cancellationToken))
			.ToDto();
}