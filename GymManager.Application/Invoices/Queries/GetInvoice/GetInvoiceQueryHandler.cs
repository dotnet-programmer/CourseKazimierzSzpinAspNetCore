using GymManager.Application.Common.Interfaces;
using GymManager.Application.Invoices.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Invoices.Queries.GetInvoice;

public class GetInvoiceQueryHandler : IRequestHandler<GetInvoiceQuery, InvoiceDto>
{
	private readonly IApplicationDbContext _context;

	public GetInvoiceQueryHandler(IApplicationDbContext context) => _context = context;

	public async Task<InvoiceDto> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
		=> (await _context
			.Invoices
			.Include(x => x.Ticket).ThenInclude(x => x.TicketType)
			.Include(x => x.User).ThenInclude(x => x.Address)
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.InvoiceId == request.Id && x.UserId == request.UserId))
			.ToDto();
}