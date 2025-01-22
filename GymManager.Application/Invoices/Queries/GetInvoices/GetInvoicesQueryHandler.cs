using GymManager.Application.Common.Interfaces;
using GymManager.Application.Invoices.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Invoices.Queries.GetInvoices;

public class GetInvoicesQueryHandler(IApplicationDbContext context) : IRequestHandler<GetInvoicesQuery, IEnumerable<InvoiceBasicsDto>>
{
	public async Task<IEnumerable<InvoiceBasicsDto>> Handle(GetInvoicesQuery request, CancellationToken cancellationToken)
		=> (await context.Invoices
			.Where(x => x.UserId == request.UserId)
			.Include(x => x.User)
			.AsNoTracking()
			.ToListAsync(cancellationToken))
			.Select(x => x.ToBasicsDto());
}