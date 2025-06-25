namespace GymManager.Application.Common.Interfaces;

// serwis wywoływany w warstwie aplikacji i będzie umożliwiał wysłanie notyfikacji za pomocą SignalR
public interface IUserNotificationService
{
	// z poziomu aplikacji będzie się tylko wywoływać tą metodę ze wskazaniem,
	// któremu userowi (userId) będzie wysłane powiadomienie o podanej treści (message)
	Task SendNotification(string userId, string message);
}