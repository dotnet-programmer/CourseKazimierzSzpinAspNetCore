using GymManager.Application.Common.Interfaces;
using GymManager.Application.Common.Models.Payments;
using GymManager.Domain.Entities;
using GymManager.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

// NuGet Package: Microsoft.Extensions.Logging.Abstractions

namespace GymManager.Application.Tickets.Commands.AddTicket;

public class AddTicketCommandHandler(
	IApplicationDbContext context,
	IDateTimeService dateTimeService,
	IPrzelewy24 przelewy24,
	IHttpContext httpContext) : IRequestHandler<AddTicketCommand, string>
{
	private readonly IApplicationDbContext _context = context;
	private readonly IDateTimeService _dateTimeService = dateTimeService;
	private readonly IPrzelewy24 _przelewy24 = przelewy24;
	private readonly IHttpContext _httpContext = httpContext;

	public async Task<string> Handle(AddTicketCommand request, CancellationToken cancellationToken)
	{
		// identyfikator sesji, potrzebny do systemu płatności
		var sessionId = Guid.NewGuid().ToString();

		// dodanie nowej transakcji przez system płatności
		var token = await AddTransactionPrzelewy24Async(request, sessionId);

		// po dodaniu nowej transakcji do systemu Przelewy24 dodanie do bazy danych informacji o nowym karnecie
		await AddToDatabaseAsync(request, sessionId, token, cancellationToken);

		return token;
	}

	private async Task<string> AddTransactionPrzelewy24Async(AddTicketCommand request, string sessionId)
	{
		var user = await _context.Users
			.AsNoTracking()
			.Select(x => new { Email = x.Email, Id = x.Id })
			.FirstOrDefaultAsync(x => x.Id == request.UserId);

		string clientEmail = user.Email;

		var response = await _przelewy24.NewTransactionAsync(
			new P24TransactionRequest
			{
				Amount = (int)(request.Price * 100),
				Country = "PL",
				Currency = "PLN",
				Description = "Karnet",
				Email = clientEmail,
				Language = "pl",
				UrlReturn = $"{_httpContext.AppBaseUrl}/Ticket/Tickets",
				UrlStatus = $"{_httpContext.AppBaseUrl}/api/ticket/updatestatus",
				SessionId = sessionId
			});

		string token = string.Empty;

		if (string.IsNullOrWhiteSpace(response.Data?.Token))
		{
			throw new Exception($"Przelewy24 havent return token. Error: {response.Error}");
		}
		else
		{
			token = response.Data.Token;
		}

		return token;
	}

	private async Task AddToDatabaseAsync(AddTicketCommand request, string sessionId, string token, CancellationToken cancellationToken)
	{
		var ticketType = _context.TicketTypes
			.AsNoTracking()
			.FirstOrDefault(x => x.TicketTypeId == request.TicketTypeId);

		var ticket = new Ticket
		{
			TicketTypeId = request.TicketTypeId,
			StartDate = request.StartDate,
			UserId = request.UserId,
			Price = ticketType.Price,
			TicketId = sessionId,
			Token = token,
			CreatedDate = _dateTimeService.Now,
			EndDate = ticketType.TicketTypeEnum switch
			{
				TicketTypeEnum.Single => request.StartDate.Date,
				TicketTypeEnum.Weekly => request.StartDate.AddDays(6).Date,
				TicketTypeEnum.Monthly => request.StartDate.AddDays(DateTime.DaysInMonth(request.StartDate.Year, request.StartDate.Month) - 1).Date,
				TicketTypeEnum.Annual => request.StartDate.AddDays(364).Date,
				_ => throw new ArgumentException("Argument Exception")
			}
		};

		_context.Tickets.Add(ticket);
		await _context.SaveChangesAsync(cancellationToken);
	}
}


//public class AddTicketCommandHandler : IRequestHandler<AddTicketCommand, string>
//{
//	//private readonly ILogger _logger;

//	//public AddTicketCommandHandler(ILogger<AddTicketCommandHandler> logger)
//	//   {
//	//	_logger = logger;
//	//}

//	//public async Task Handle(AddTicketCommand request, CancellationToken cancellationToken)
//	//{
//	//	// tutaj można np. dodać do bazy nowy obiekt
//	//	//var ticket = new Ticket();
//	//	//ticket.Name = request.Name;
//	//	// zapis w bd
//	//	// jeśli nic się nie zwraca, to zwraca się Unit.Value - UWAGA!!! tylko do MediatR wersji 11, od 12 zwraca się Task.CompletedTask
//	//	//return Unit.Value;

//	//	_logger.LogInformation("Log from AddTicketCommandHandler");

//	//	await Task.CompletedTask;
//	//}

//	//public Task Handle(AddTicketCommand request, CancellationToken cancellationToken)
//	//{
//	//	// tutaj można np. dodać do bazy nowy obiekt
//	//	//var ticket = new Ticket();
//	//	//ticket.Name = request.Name;
//	//	// zapis w bd
//	//	// jeśli nic się nie zwraca, to zwraca się Unit.Value - UWAGA!!! tylko do MediatR wersji 11, od 12 zwraca się Task.CompletedTask
//	//	//return Unit.Value;

//	//	_logger.LogInformation("Log from AddTicketCommandHandler");

//	//	return Task.CompletedTask;
//	//}
//	public async Task<string> Handle(AddTicketCommand request, CancellationToken cancellationToken)
//	{
//		throw new NotImplementedException();
//	}
//}