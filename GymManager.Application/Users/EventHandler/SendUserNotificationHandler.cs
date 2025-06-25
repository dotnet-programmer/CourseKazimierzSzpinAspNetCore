using GymManager.Application.Common.Events;
using GymManager.Application.Common.Interfaces;
using GymManager.Application.Tickets.Events;

namespace GymManager.Application.Users.EventHandler;

// eventy
// 6. handler, który zostanie podpięty pod zdarzenie TicketPaidEvent
// jego metoda HandleAsync zostanie wywołana gdy faktura zostanie opłacona,
// wtedy zostanie również wysłane powiadomienie użytkownikowi
public class SendUserNotificationHandler(IUserNotificationService userNotificationService) : IEventHandler<TicketPaidEvent>
{
	public async Task HandleAsync(TicketPaidEvent @event)
	{
		await Task.Delay(2000);
		await userNotificationService.SendNotification(@event.UserId, "Płatność została potwierdzona, dziękujemy za zakup karnetu.");
	}
}