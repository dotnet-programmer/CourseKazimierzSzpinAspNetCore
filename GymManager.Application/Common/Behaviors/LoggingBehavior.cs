using GymManager.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GymManager.Application.Common.Behaviors;

// Logowanie wszystkich requestów aplikacji
public class LoggingBehavior<TRequest, TResponse>(ILogger<TRequest> logger, ICurrentUserService currentUserService) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
	// metoda wywoływana przed każdym requestem
	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		// pobranie nazwy requesta
		var requestName = typeof(TRequest).Name;

		// Pobieranie informacji o zalogowanym użytkowniku bez zapytań na bazie danych
		string userId = currentUserService.UserId ?? string.Empty;
		string userName = currentUserService.UserName ?? string.Empty;

		//logger.LogInformation($"Handling {requestName}");
		logger.LogInformation("Handling {Name}", requestName);

		// bez dodania @ loguje się taki wpis: INFO GymManager Request: GetTicketByIdQuery   GymManager.Application.Tickets.Queries.GetTicketById.GetTicketByIdQuery
		// po dodaniu @ loguje się taki wpis: INFO GymManager Request: "GetTicketByIdQuery" "" "" {"TicketId":1}
		logger.LogInformation("GymManager Request: {Name} {UserId} {UserName} {Request}", requestName, userId, userName, request);
		logger.LogInformation("GymManager Request: {@Name} {@UserId} {@UserName} {@Request}", requestName, userId, userName, request);

		// przypisanie do responsa odpowiedzi obsłużonego requesta
		var response = await next();

		// informacja, że request został obsłużony
		logger.LogInformation("Handled {Name}", typeof(TResponse).Name);
		logger.LogInformation("Handled {@Name}", typeof(TResponse).Name);

		// zwrócenie odpowiedzi
		return response;
	}
}