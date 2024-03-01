using GymManager.Application.Common.Interfaces;
using GymManager.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Invoices.Commands.AddInvoice;

public class AddInvoiceCommandsHandler : IRequestHandler<AddInvoiceCommand, int>
{
	private readonly IApplicationDbContext _context;
	private readonly IDateTimeService _dateTimeService;

	public AddInvoiceCommandsHandler(IApplicationDbContext context, IDateTimeService dateTimeService)
	{
		_context = context;
		_dateTimeService = dateTimeService;
	}

	public async Task<int> Handle(AddInvoiceCommand request, CancellationToken cancellationToken)
	{
		// pobierz informacje o karnecie
		var ticket = await _context
			.Tickets
			.FirstOrDefaultAsync(x => x.TicketId == request.TicketId);

		// utwórz nową fakturę
		Invoice invoice = new()
		{
			CreatedDate = _dateTimeService.Now,
			MethodOfPayment = "Przelew",
			Month = _dateTimeService.Now.Month,
			Year = _dateTimeService.Now.Year,
			UserId = request.UserId,
			Value = ticket.Price,
			TicketId = request.TicketId
		};

		// dodaj fakturę do bazy danych
		_context.Invoices.Add(invoice);

		// zapisz zmiany w bazie danych
		await _context.SaveChangesAsync(cancellationToken);

		// zwróć ID utworzonej faktury
		return invoice.InvoiceId;
	}
}