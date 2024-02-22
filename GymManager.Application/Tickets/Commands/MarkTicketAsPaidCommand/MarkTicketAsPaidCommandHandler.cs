using GymManager.Application.Common.Interfaces;
using GymManager.Application.Common.Models.Payments;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GymManager.Application.Tickets.Commands.MarkTicketAsPaidCommand;

public class MarkTicketAsPaidCommandHandler : IRequestHandler<MarkTicketAsPaidCommand>
{
	private readonly IApplicationDbContext _context;
	private readonly IPrzelewy24 _przelewy24;
	private readonly ILogger _logger;

	public MarkTicketAsPaidCommandHandler(
		IApplicationDbContext context,
		IPrzelewy24 przelewy24,
		ILogger<MarkTicketAsPaidCommandHandler> logger)
	{
		_context = context;
		_przelewy24 = przelewy24;
		_logger = logger;
	}

	public async Task Handle(MarkTicketAsPaidCommand request, CancellationToken cancellationToken)
	{
		_logger.LogInformation($"Przelewy24 - payment verification started - {request.SessionId}");
		await VerifyTransactionPrzelewy24(request);
		await UpdatePaymentInDb(request.SessionId, cancellationToken);
		_logger.LogInformation($"Przelewy24 - payment verification finished - {request.SessionId}");
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
	private async Task UpdatePaymentInDb(string sessionId, CancellationToken cancellationToken)
	{
		var ticket = await _context.Tickets.FirstOrDefaultAsync(x => x.TicketId == sessionId);
		ticket.IsPaid = true;
		await _context.SaveChangesAsync(cancellationToken);
	}
}