using GymManager.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Invoices.Commands.EditInvoice;

public class EditInvoiceCommandHandler(IApplicationDbContext context) : IRequestHandler<EditInvoiceCommand>
{
	public async Task Handle(EditInvoiceCommand request, CancellationToken cancellationToken)
	{
		// pobranie faktury do aktualizacji, jeśli brak faktury o podanym ID to wyjątek
		var invoice = await context.Invoices
			.FirstOrDefaultAsync(x => x.InvoiceId == request.Id && x.UserId == request.UserId, cancellationToken)
			?? throw new Exception("Invoice NotFound");

		// aktualizacja danych
		invoice.Value = request.Value;

		// zapis zmian
		await context.SaveChangesAsync(cancellationToken);
	}
}