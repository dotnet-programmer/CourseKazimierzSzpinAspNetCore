using GymManager.Application.Common.Events;
using GymManager.Application.Common.Interfaces;
using GymManager.Application.Common.Models.Payments;
using GymManager.Domain.Entities;
using GymManager.Application.Tickets.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GymManager.Application.Tickets.Commands.MarkTicketAsPaidCommand;

// oznaczenie faktury jako opłaconej
public class MarkTicketAsPaidCommandHandler(
	IApplicationDbContext context,
	IPrzelewy24 przelewy24,
	ILogger<MarkTicketAsPaidCommandHandler> logger,
	//IGymInvoices gymInvoices,
	//IUserNotificationService userNotificationService,
	IEventDispatcher eventDispatcher
	) : IRequestHandler<MarkTicketAsPaidCommand, Unit>
{
	private readonly IApplicationDbContext _context = context;
	private readonly IPrzelewy24 _przelewy24 = przelewy24;
	private readonly ILogger _logger = logger;
	//private readonly IGymInvoices _gymInvoices = gymInvoices;
	//private readonly IUserNotificationService _userNotificationService = userNotificationService;
	private readonly IEventDispatcher _eventDispatcher = eventDispatcher;

	// eventy
	// w tej metodzie były 3 odpowiedzialności:
	// 1. weryfikacja płatności w systemie Przelewy24 i aktualizacja bazy danych z oznaczeniem karnetu jako zapłacony
	// 2. dodanie nowej faktury poprzez Api
	// 3. wysłanie notyfikacji do klienta poprzez SignalR
	// żeby być zgodnym z zasadą pojedynczej odpowiedzialności (Single Responsibility Principle) pkt 2 i 3 zostały przeniesione do eventów,
	public async Task<Unit> Handle(MarkTicketAsPaidCommand request, CancellationToken cancellationToken)
	{
		// na początku zgodnie z dokumentacją Przelewy24 należy zwalidować adres IP z którego przychodzi request
		// dlatego dodana jest walidacja w MarkTicketAsPaidCommandValidator

		// zalogowanie informacji o nadchodzącym requeście
		_logger.LogInformation($"Przelewy24 - payment verification started - {request.SessionId}");
		
		// wywołanie metody weryfikacyjnej
		await VerifyTransactionPrzelewy24Async(request);

		var ticket = await _context.Tickets.FirstOrDefaultAsync(x => x.TicketId == request.SessionId, cancellationToken);

		// jeżeli płatność się powiodła, to w bazie danych zostanie oznaczony karnet jako zapłacony
		await UpdatePaymentInDbAsync(ticket, cancellationToken);
		
		// zalogowanie informacji o poprawnej płatności
		_logger.LogInformation($"Przelewy24 - payment verification finished - {request.SessionId}");

		#region metody przeniesione do eventów dlatego w komentarzach
		// jeśli płatność została zweryfikowana to dodanie nowej faktury poprzez Api
		//await _gymInvoices.AddInvoice(ticket.TicketId);

		// SingalR - jeśli karnet zostanie opłacony to wyślij notyfikację dla klienta
		//await Task.Delay(2000); // sztuczne opóźnienie, żeby notyfikacja nie została wyświetlona od razu - czas na powrót na stronę klienta po płatności
		//await _userNotificationService.SendNotification(ticket.UserId, "Płatność została potwierdzona, dziękujemy za zakup karnetu.");

		#endregion metody przeniesione do eventów dlatego w komentarzach

		// opublikowanie eventu TicketPaidEvent,
		// dzięki temu zostaną wywołane handlery które podpięły się pod ten event
		// to rozwiązanie ma jedną wadę - pracując na eveach nie ma dostępu do kontekstu z requesta,
		// dlatego nie da się pobrać wewnątrz tych klas np. Id zalogowanego użytkownika
		await _eventDispatcher.PublishAsync(new TicketPaidEvent
		{
			TicketId = ticket.TicketId,
			UserId = ticket.UserId,
		});

		return Unit.Value;
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