using GymManager.Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace GymManager.Infrastructure.SignalR.UserNotification;

// serwis wywoływany w warstwie aplikacji i będzie umożliwiał wysłanie notyfikacji za pomocą SignalR
public class UserNotificationService(
	// NotificationUserHub - klasa dziedzicząca po klasie Hub
	IHubContext<NotificationUserHub> hubContext,
	IUserConnectionManager userConnectionManager
	) : IUserNotificationService
{
	// z poziomu aplikacji będzie się tylko wywoływać tą metodę ze wskazaniem,
	// któremu userowi (userId) będzie wysłane powiadomienie o podanej treści (message)
	public async Task SendNotification(string userId, string message)
	{
		// pobierz wszystkie połączenia dla wybranego usera
		// czyli jeśli ma otwarte 2 przeglądarki, to powiadomienie wyświetli się w obu przeglądarkach
		var allUserConnections = userConnectionManager.GetUserConnections(userId);

		// jeśeli null lub brak połączeń to wyjdź
		if (allUserConnections == null || allUserConnections.Count == 0)
		{
			return;
		}

		// dla każdego połączenia wyślij powiadomienie
		foreach (var connectionInfo in allUserConnections)
		{
			// parametr "sendToUser" - wskazanie metody w JavaScript
			// w projekcie UI w Libmanie instalacja z cdnjs pakietu microsoft-signalr
			// przykład użycia w _Layout.cshtml
			await hubContext.Clients.Client(connectionInfo).SendAsync("sendToUser", message);
		}
	}
}