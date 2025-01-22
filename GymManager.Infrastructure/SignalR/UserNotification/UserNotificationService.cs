using GymManager.Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace GymManager.Infrastructure.SignalR.UserNotification;

// serwis wywoływany w warstwie aplikacji i będzie umożliwiał wysłanie notyfikacji za pomocą SignalR
// z poziomu aplikacji będzie się tylko wywoływać tą metodę ze wskazaniem,
// któremu userowi będzie wysłane powiadomienie o podanej treści
public class UserNotificationService(
	IHubContext<NotificationUserHub> hubContext,
	IUserConnectionManager userConnectionManager) : IUserNotificationService
{
	private readonly IHubContext<NotificationUserHub> _hubContext = hubContext;
	private readonly IUserConnectionManager _userConnectionManager = userConnectionManager;

	public async Task SendNotification(string userId, string message)
	{
		// pobierz wszystkie połączenia dla wybranego usera
		// czyli jeśli ma otwarte 2 przeglądarki, to powiadomienie wyświetli się w obu przeglądarkach
		var connection = _userConnectionManager.GetUserConnections(userId);

		// jeśeli null lub brak połączeń to wyjdź
		if (connection == null || !connection.Any())
		{
			return;
		}

		// dla każdego połaczenia wyślij powiadomienie
		foreach (var connectionInfo in connection)
		{
			// parametr "sendToUser" - wskazanie metody w JavaScript
			await _hubContext.Clients.Client(connectionInfo).SendAsync("sendToUser", message);
		}
	}
}