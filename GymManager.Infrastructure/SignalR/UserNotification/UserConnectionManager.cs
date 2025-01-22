using GymManager.Application.Common.Interfaces;

namespace GymManager.Infrastructure.SignalR.UserNotification;

// SignalR - natychmiastowe notyfikacja bez odświeżania strony z serwera do klientów i dowolnego użytkownika
// serwis do pobierania informacji o aktualnych użytkownikach i ich połączeniach
// 3 metody do zarządzania połączeniami użytkownika
public class UserConnectionManager : IUserConnectionManager
{
	// zmienna żeby dostęp był tylko z 1 wątku
	private static readonly string _userConnectionMapLocker = string.Empty;

	// słownik przechowujący info o wszystkich użytkownikach wraz z info o ich połączeniach
	// Dictionary<string, List<string>>
	// - string - Id usera,
	// - List<string> - lista połączeń dla każdego usera
	private static readonly Dictionary<string, List<string>> _userConnectionMap = [];

	// jeśli ktoś wejdzie na stronę, to zapisz informacje o Id usera oraz IdConnection z contextu
	// trzeba przechować te informacje na serwerze, tak żwby wysłać odpowiednią wiadomość do odpowiedniego usera w przeglądarce
	public void KeepUserConnection(string userId, string connectionId)
	{
		// zablokuj wątek
		lock (_userConnectionMapLocker)
		{
			// jeżeli w słowniku nie ma żadnego połączenia dla usera to utwórz nową listę dla tego usera
			if (!_userConnectionMap.ContainsKey(userId))
			{
				_userConnectionMap[userId] = [];
			}

			// dodaj info o połączeniu dla klucza UserId
			_userConnectionMap[userId].Add(connectionId);
		}
	}

	// jeśli ktoś opuszcza stronę to trzeba usunąć informacje z listy
	public void RemoveUserConnection(string connectionId)
	{
		// zablokuj wątek
		lock (_userConnectionMapLocker)
		{
			// przejdź po wszystkich użytkownikach
			foreach (var userId in _userConnectionMap.Keys)
			{
				// jeżeli słownik zawiera info o tym userze
				if (_userConnectionMap.ContainsKey(userId))
				{
					// jeżeli user zawiera połączenia
					if (_userConnectionMap[userId].Contains(connectionId))
					{
						// to usuń połączenia
						_userConnectionMap[userId].Remove(connectionId);
						break;
					}
				}
			}
		}
	}

	// pobieranie informacji o połączeniach dla danego usera
	public List<string> GetUserConnections(string userId)
	{
		List<string> connections = [];

		try
		{
			// zablokuj wątek
			lock (_userConnectionMapLocker)
			{
				// pobierz info o wszystkich połączeniach dla danego usera
				connections = _userConnectionMap[userId];
			}
		}
		catch
		{
		}

		return connections;
	}
}