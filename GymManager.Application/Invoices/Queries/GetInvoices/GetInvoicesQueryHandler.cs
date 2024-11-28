using GymManager.Application.Common.Interfaces;
using GymManager.Application.Invoices.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Invoices.Queries.GetInvoices;
public class GetInvoicesQueryHandler : IRequestHandler<GetInvoicesQuery, IEnumerable<InvoiceBasicsDto>>
{
	private readonly IApplicationDbContext _context;

	public GetInvoicesQueryHandler(IApplicationDbContext context) =>
		_context = context;

	public async Task<IEnumerable<InvoiceBasicsDto>> Handle(GetInvoicesQuery request, CancellationToken cancellationToken)
		=> (await _context
			.Invoices
			.Where(x => x.UserId == request.UserId)
			.Include(x => x.User)
			.AsNoTracking()
			.ToListAsync())
			.Select(x => x.ToBasicsDto());
}