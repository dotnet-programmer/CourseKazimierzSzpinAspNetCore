using GymManager.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Invoices.Commands.DeleteInvoice;

public class DeleteInvoiceCommandHandler : IRequestHandler<DeleteInvoiceCommand>
{
	private readonly IApplicationDbContext _context;

	public DeleteInvoiceCommandHandler(IApplicationDbContext context) =>
		_context = context;

	public async Task Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
	{
		var invoice = await _context
			.Invoices
			.FirstOrDefaultAsync(x => x.InvoiceId == request.Id && x.UserId == request.UserId);

		if (invoice == null)
		{
			throw new Exception("Invoice NotFound");
		}

		_context.Invoices.Remove(invoice);
		await _context.SaveChangesAsync(cancellationToken);

		return;
	}
}