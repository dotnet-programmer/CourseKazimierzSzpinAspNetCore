using GymManager.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Invoices.Commands.DeleteInvoice;

public class DeleteInvoiceCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteInvoiceCommand>
{
	public async Task Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
	{
		var invoice = await context.Invoices
			.FirstOrDefaultAsync(x => x.InvoiceId == request.Id && x.UserId == request.UserId, cancellationToken) 
			?? throw new Exception("Invoice NotFound");
		context.Invoices.Remove(invoice);
		await context.SaveChangesAsync(cancellationToken);
	}
}