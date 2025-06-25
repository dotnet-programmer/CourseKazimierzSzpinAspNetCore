namespace GymManager.Application.Common.Interfaces;

// SignalR - natychmiastowe notyfikacja bez odświeżania strony z serwera do klientów i dowolnego użytkownika
// serwis do pobierania informacji o aktualnych użytkownikach i ich połączeniach
// 3 metody do zarządzania połączeniami użytkownika
public interface IUserConnectionManager
{
	// jeśli ktoś wejdzie na stronę, to zapisz informacje o Id usera oraz IdConnection z contextu
	// trzeba przechować te informacje na serwerze, tak żeby wysłać odpowiednią wiadomość do odpowiedniego usera (klienta) w przeglądarce
	void KeepUserConnection(string userId, string connectionId);

	// jeśli ktoś opuszcza stronę to trzeba usunąć informacje z listy
	void RemoveUserConnection(string connectionId);

	// metoda pobierająca informacje o połączeniach użytkownika
	List<string> GetUserConnections(string userId);
}