using GymManager.Application.Common.Events;
using GymManager.Application.Common.Interfaces;
using GymManager.Application.Common.Models.Payments;
using GymManager.Application.Tickets.Events;
using GymManager.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GymManager.Application.Tickets.Commands.MarkTicketAsPaidCommand;

public class MarkTicketAsPaidCommandHandler(
	IApplicationDbContext context,
	IPrzelewy24 przelewy24,
	ILogger<MarkTicketAsPaidCommandHandler> logger,
	IGymInvoices gymInvoices,
	IUserNotificationService userNotificationService,
	IEventDispatcher eventDispatcher) : IRequestHandler<MarkTicketAsPaidCommand>
{
	private readonly IApplicationDbContext _context = context;
	private readonly IPrzelewy24 _przelewy24 = przelewy24;
	private readonly ILogger _logger = logger;
	//private readonly IGymInvoices _gymInvoices = gymInvoices;
	//private readonly IUserNotificationService _userNotificationService = userNotificationService;
	private readonly IEventDispatcher _eventDispatcher = eventDispatcher;

	public async Task Handle(MarkTicketAsPaidCommand request, CancellationToken cancellationToken)
	{
		#region oznaczenie faktury jako opłaconej
		_logger.LogInformation($"Przelewy24 - payment verification started - {request.SessionId}");
		await VerifyTransactionPrzelewy24Async(request);
		var ticket = await _context.Tickets.FirstOrDefaultAsync(x => x.TicketId == request.SessionId, cancellationToken);
		await UpdatePaymentInDbAsync(ticket, cancellationToken);
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
	}

	private async Task VerifyTransactionPrzelewy24Async(MarkTicketAsPaidCommand request)
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
	private async Task UpdatePaymentInDbAsync(Ticket ticket, CancellationToken cancellationToken)
	{
		ticket.IsPaid = true;
		await _context.SaveChangesAsync(cancellationToken);
	}
}