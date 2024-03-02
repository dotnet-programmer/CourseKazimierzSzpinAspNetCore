namespace GymManager.Application.Common.Interfaces;

// serwis wywoływany w warstwie aplikacji i będzie umożliwiał wysłanie notyfikacji za pomocą SignalR
// z poziomu aplikacji będzie się tylko wywoływać tą metodę ze wskazaniem,
// któremu userowi będzie wysłane powiadomienie o podanej treści
public interface IUserNotificationService
{
	Task SendNotification(string userId, string message);
}