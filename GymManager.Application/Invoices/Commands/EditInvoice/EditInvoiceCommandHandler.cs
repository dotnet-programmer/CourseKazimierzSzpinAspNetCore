using GymManager.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Invoices.Commands.EditInvoice;

public class EditInvoiceCommandHandler : IRequestHandler<EditInvoiceCommand>
{
	private readonly IApplicationDbContext _context;

	public EditInvoiceCommandHandler(IApplicationDbContext context) => _context = context;

	public async Task Handle(EditInvoiceCommand request, CancellationToken cancellationToken)
	{
		// pobranie faktury do aktualizacji
		var invoice = await _context
			.Invoices
			.FirstOrDefaultAsync(x => x.InvoiceId == request.Id && x.UserId == request.UserId);

		// jeśli brak faktury o podanym ID to wyjątek
		if (invoice == null)
		{
			throw new Exception("Invoice NotFound");
		}

		// aktualizacja danych
		invoice.Value = request.Value;

		// zapis zmian
		await _context.SaveChangesAsync(cancellationToken);
		return;
	}
}