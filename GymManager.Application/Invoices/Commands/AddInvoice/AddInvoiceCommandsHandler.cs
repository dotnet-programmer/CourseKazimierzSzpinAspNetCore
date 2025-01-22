using GymManager.Application.Common.Interfaces;
using GymManager.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Invoices.Commands.AddInvoice;

public class AddInvoiceCommandsHandler(IApplicationDbContext context, IDateTimeService dateTimeService) : IRequestHandler<AddInvoiceCommand, int>
{
	public async Task<int> Handle(AddInvoiceCommand request, CancellationToken cancellationToken)
	{
		// pobierz informacje o karnecie
		var ticket = await context.Tickets
			.FirstOrDefaultAsync(x => x.TicketId == request.TicketId, cancellationToken);

		// utwórz nową fakturę
		Invoice invoice = new()
		{
			CreatedDate = dateTimeService.Now,
			MethodOfPayment = "Przelew",
			Month = dateTimeService.Now.Month,
			Year = dateTimeService.Now.Year,
			UserId = request.UserId,
			Value = ticket.Price,
			TicketId = request.TicketId
		};

		// dodaj fakturę do bazy danych
		context.Invoices.Add(invoice);

		// zapisz zmiany w bazie danych
		await context.SaveChangesAsync(cancellationToken);

		// zwróć ID utworzonej faktury
		return invoice.InvoiceId;
	}
}