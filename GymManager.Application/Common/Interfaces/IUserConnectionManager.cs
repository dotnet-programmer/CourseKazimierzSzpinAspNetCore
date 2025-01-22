namespace GymManager.Application.Common.Interfaces;

// SignalR - natychmiastowe notyfikacja bez odświeżania strony z serwera do klientów i dowolnego użytkownika
// serwis do pobierania informacji o aktualnych użytkownikach i ich połączeniach
// 3 metody do zarządzania połączeniami użytkownika
public interface IUserConnectionManager
{
	void KeepUserConnection(string userId, string connectionId);
	void RemoveUserConnection(string connectionId);
	List<string> GetUserConnections(string userId);
}