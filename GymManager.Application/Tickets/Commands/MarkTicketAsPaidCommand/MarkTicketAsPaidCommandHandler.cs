using GymManager.Application.Common.Events;
using GymManager.Application.Common.Interfaces;
using GymManager.Application.Common.Models.Payments;
using GymManager.Application.Tickets.Events;
using GymManager.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GymManager.Application.Tickets.Commands.MarkTicketAsPaidCommand;

public class MarkTicketAsPaidCommandHandler : IRequestHandler<MarkTicketAsPaidCommand>
{
	private readonly IApplicationDbContext _context;
	private readonly IPrzelewy24 _przelewy24;
	private readonly ILogger _logger;
	private readonly IGymInvoices _gymInvoices;
	private readonly IUserNotificationService _userNotificationService;
	private readonly IEventDispatcher _eventDispatcher;

	public MarkTicketAsPaidCommandHandler(
		IApplicationDbContext context,
		IPrzelewy24 przelewy24,
		ILogger<MarkTicketAsPaidCommandHandler> logger,
		IGymInvoices gymInvoices,
		IUserNotificationService userNotificationService,
		IEventDispatcher eventDispatcher)
	{
		_context = context;
		_przelewy24 = przelewy24;
		_logger = logger;
		_gymInvoices = gymInvoices;
		_userNotificationService = userNotificationService;
		_eventDispatcher = eventDispatcher;
	}

	public async Task Handle(MarkTicketAsPaidCommand request, CancellationToken cancellationToken)
	{
		#region oznaczenie faktury jako opłaconej
		_logger.LogInformation($"Przelewy24 - payment verification started - {request.SessionId}");
		await VerifyTransactionPrzelewy24(request);
		var ticket = await _context.Tickets.FirstOrDefaultAsync(x => x.TicketId == request.SessionId);
		await UpdatePaymentInDb(ticket, cancellationToken);
		_logger.LogInformation($"Przelewy24 - payment verification finished - {request.SessionId}");
		#endregion oznaczenie faktury jako opłaconej

		#region metody przeniesione do eventów dlatego w komentarzach
		// jeśli płatność została zweryfikowana to dodanie nowej faktury poprzez Api
		//await _gymInvoices.AddInvoice(ticket.TicketId);

		// SingalR - jeśli karnet zostanie opłacony to wyślij notyfikację dla klienta
		//await Task.Delay(2000);
		//await _userNotificationService.SendNotification(ticket.UserId, "Płatność została potwierdzona, dziękujemy za zakup karnetu.");

		// opublikowanie eventu TicketPaidEvent,
		// dzięki temu zostaną wywołane handlery które podpięły się pod ten event
		await _eventDispatcher.PublishAsync(new TicketPaidEvent
		{
			TicketId = ticket.TicketId,
			UserId = ticket.UserId,
		});

		#endregion metody przeniesione do eventów dlatego w komentarzach

		return;
	}

	private async Task VerifyTransactionPrzelewy24(MarkTicketAsPaidCommand request)
	{
		var response = await _przelewy24.TransactionVerifyAsync(
			new P24TransactionVerifyRequest
			{
				Amount = request.Amount,
				Currency = request.Currency,
				MerchantId = request.MerchantId,
				PosId = request.PosId,
				OrderId = request.OrderId,
				SessionId = request.SessionId,
			});

		if (response.Data?.Status != "success")
		{
			throw new Exception("Invalid payment status in przelewy24");
		}
	}

	// oznaczenie karnetu w bazie jako zapłacony
	private async Task UpdatePaymentInDb(Ticket ticket, CancellationToken cancellationToken)
	{
		ticket.IsPaid = true;
		await _context.SaveChangesAsync(cancellationToken);
	}
}